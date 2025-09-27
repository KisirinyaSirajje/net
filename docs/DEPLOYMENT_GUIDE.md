# Deployment Guide

## Overview

This guide provides step-by-step instructions for deploying the Uganda Curriculum School Management System in various environments.

## Prerequisites

### System Requirements

**Minimum Server Requirements:**
- CPU: 2 cores, 2.4 GHz
- RAM: 4 GB
- Storage: 50 GB SSD
- OS: Windows Server 2019+ or Linux (Ubuntu 18.04+)

**Recommended Server Requirements:**
- CPU: 4 cores, 2.8 GHz
- RAM: 8 GB
- Storage: 100 GB SSD
- OS: Windows Server 2022 or Ubuntu 22.04 LTS

**Software Requirements:**
- .NET 8.0 Runtime
- ASP.NET Core 8.0
- SQLite (included) or SQL Server 2019+
- IIS (Windows) or Nginx/Apache (Linux)

## Deployment Options

### 1. Local Development Deployment

#### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Git

#### Steps

1. **Clone Repository**
   ```bash
   git clone https://github.com/your-repo/uganda-school-lms.git
   cd uganda-school-lms/SchoolManagementSystem
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Update Database**
   ```bash
   dotnet ef database update
   ```

4. **Run Application**
   ```bash
   dotnet run
   ```

5. **Access Application**
   - Navigate to `https://localhost:5001`
   - Default admin credentials: admin@school.edu.ug / Admin123!

### 2. Windows Server Deployment (IIS)

#### Prerequisites
- Windows Server 2019 or later
- IIS with ASP.NET Core Module
- .NET 8.0 Runtime

#### Installation Steps

1. **Install IIS and ASP.NET Core Module**
   ```powershell
   # Enable IIS
   Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole, IIS-WebServer, IIS-CommonHttpFeatures, IIS-HttpErrors, IIS-HttpLogging, IIS-RequestFiltering, IIS-StaticContent, IIS-DefaultDocument

   # Install ASP.NET Core Module
   # Download from https://dotnet.microsoft.com/download/dotnet/8.0
   ```

2. **Install .NET 8.0 Runtime**
   ```powershell
   # Download and install from Microsoft
   # https://dotnet.microsoft.com/download/dotnet/8.0
   ```

3. **Prepare Application**
   ```bash
   # Build and publish
   dotnet publish -c Release -o C:\inetpub\wwwroot\SchoolLMS
   ```

4. **Configure IIS**
   ```powershell
   # Create Application Pool
   New-IISAppPool -Name "SchoolLMS"
   Set-IISAppPool -Name "SchoolLMS" -ManagedRuntimeVersion ""

   # Create Website
   New-IISSite -Name "SchoolLMS" -PhysicalPath "C:\inetpub\wwwroot\SchoolLMS" -Port 80 -ApplicationPool "SchoolLMS"
   ```

5. **Configure Database Connection**
   - Update `appsettings.Production.json`
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=C:\\Data\\SchoolManagement.db"
     }
   }
   ```

6. **Set Permissions**
   ```powershell
   # Grant IIS_IUSRS access to application folder
   icacls "C:\inetpub\wwwroot\SchoolLMS" /grant "IIS_IUSRS:(OI)(CI)F" /T
   ```

#### SSL Configuration

1. **Request SSL Certificate**
   - Use Let's Encrypt or purchase from CA
   - Install certificate in IIS

2. **Configure HTTPS Binding**
   ```powershell
   New-IISSiteBinding -Name "SchoolLMS" -Protocol https -Port 443 -CertificateThumbPrint "YOUR_CERT_THUMBPRINT"
   ```

3. **Force HTTPS Redirect**
   - Add URL Rewrite rule in web.config

### 3. Linux Deployment (Ubuntu/Nginx)

#### Prerequisites
- Ubuntu 22.04 LTS
- .NET 8.0 Runtime
- Nginx
- Supervisor (for process management)

#### Installation Steps

1. **Install .NET 8.0**
   ```bash
   wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   sudo apt-get update
   sudo apt-get install -y aspnetcore-runtime-8.0
   ```

2. **Install Nginx**
   ```bash
   sudo apt-get install nginx
   sudo systemctl start nginx
   sudo systemctl enable nginx
   ```

3. **Create Application Directory**
   ```bash
   sudo mkdir -p /var/www/schoollms
   sudo chown -R www-data:www-data /var/www/schoollms
   ```

4. **Deploy Application**
   ```bash
   # On development machine
   dotnet publish -c Release -o ./publish

   # Copy to server
   scp -r ./publish/* user@server:/var/www/schoollms/
   ```

5. **Configure Nginx**
   ```nginx
   # /etc/nginx/sites-available/schoollms
   server {
       listen 80;
       server_name your-domain.com www.your-domain.com;
       
       location / {
           proxy_pass http://localhost:5000;
           proxy_http_version 1.1;
           proxy_set_header Upgrade $http_upgrade;
           proxy_set_header Connection keep-alive;
           proxy_set_header Host $host;
           proxy_set_header X-Real-IP $remote_addr;
           proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
           proxy_set_header X-Forwarded-Proto $scheme;
           proxy_cache_bypass $http_upgrade;
       }
   }
   ```

6. **Enable Site**
   ```bash
   sudo ln -s /etc/nginx/sites-available/schoollms /etc/nginx/sites-enabled/
   sudo nginx -t
   sudo systemctl reload nginx
   ```

7. **Configure Supervisor**
   ```ini
   # /etc/supervisor/conf.d/schoollms.conf
   [program:schoollms]
   command=/usr/bin/dotnet /var/www/schoollms/SchoolManagementSystem.dll
   directory=/var/www/schoollms
   autostart=true
   autorestart=true
   stderr_logfile=/var/log/schoollms.err.log
   stdout_logfile=/var/log/schoollms.out.log
   environment=ASPNETCORE_ENVIRONMENT=Production
   user=www-data
   stopsignal=INT
   ```

8. **Start Services**
   ```bash
   sudo supervisorctl reread
   sudo supervisorctl update
   sudo supervisorctl start schoollms
   ```

### 4. Docker Deployment

#### Prerequisites
- Docker Engine 20.10+
- Docker Compose 2.0+

#### Docker Configuration

1. **Create Dockerfile**
   ```dockerfile
   # /Dockerfile
   FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
   WORKDIR /app
   EXPOSE 80
   EXPOSE 443

   FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
   WORKDIR /src
   COPY ["SchoolManagementSystem/SchoolManagementSystem.csproj", "SchoolManagementSystem/"]
   RUN dotnet restore "SchoolManagementSystem/SchoolManagementSystem.csproj"
   COPY . .
   WORKDIR "/src/SchoolManagementSystem"
   RUN dotnet build "SchoolManagementSystem.csproj" -c Release -o /app/build

   FROM build AS publish
   RUN dotnet publish "SchoolManagementSystem.csproj" -c Release -o /app/publish

   FROM base AS final
   WORKDIR /app
   COPY --from=publish /app/publish .
   ENTRYPOINT ["dotnet", "SchoolManagementSystem.dll"]
   ```

2. **Create Docker Compose**
   ```yaml
   # docker-compose.yml
   version: '3.8'

   services:
     schoollms:
       build: .
       ports:
         - "80:80"
         - "443:443"
       environment:
         - ASPNETCORE_ENVIRONMENT=Production
         - ConnectionStrings__DefaultConnection=Data Source=/data/SchoolManagement.db
       volumes:
         - ./data:/data
         - ./logs:/app/logs
       restart: unless-stopped

     nginx:
       image: nginx:alpine
       ports:
         - "8080:80"
       volumes:
         - ./nginx.conf:/etc/nginx/nginx.conf
       depends_on:
         - schoollms
       restart: unless-stopped
   ```

3. **Deploy with Docker Compose**
   ```bash
   # Build and start
   docker-compose up -d

   # View logs
   docker-compose logs -f

   # Stop services
   docker-compose down
   ```

### 5. Cloud Deployment (Azure)

#### Azure App Service Deployment

1. **Prerequisites**
   - Azure subscription
   - Azure CLI installed

2. **Create Resources**
   ```bash
   # Login to Azure
   az login

   # Create resource group
   az group create --name SchoolLMS-RG --location "East US"

   # Create App Service Plan
   az appservice plan create --name SchoolLMS-Plan --resource-group SchoolLMS-RG --sku B1

   # Create Web App
   az webapp create --name uganda-schoollms --resource-group SchoolLMS-RG --plan SchoolLMS-Plan --runtime "DOTNETCORE|8.0"
   ```

3. **Configure Database**
   ```bash
   # Create SQL Database (optional)
   az sql server create --name schoollms-server --resource-group SchoolLMS-RG --location "East US" --admin-user sqladmin --admin-password YourPassword123!

   az sql db create --name SchoolLMS --server schoollms-server --resource-group SchoolLMS-RG --service-objective Basic
   ```

4. **Configure Application Settings**
   ```bash
   az webapp config appsettings set --name uganda-schoollms --resource-group SchoolLMS-RG --settings \
     ASPNETCORE_ENVIRONMENT=Production \
     ConnectionStrings__DefaultConnection="Server=schoollms-server.database.windows.net;Database=SchoolLMS;User Id=sqladmin;Password=YourPassword123!;"
   ```

5. **Deploy Application**
   ```bash
   # Build and publish
   dotnet publish -c Release

   # Create deployment package
   cd bin/Release/net8.0/publish
   zip -r ../deploy.zip .

   # Deploy to Azure
   az webapp deployment source config-zip --name uganda-schoollms --resource-group SchoolLMS-RG --src ../deploy.zip
   ```

#### Azure Container Instances

1. **Build Container Image**
   ```bash
   # Login to Azure Container Registry
   az acr login --name yourregistry

   # Build and push image
   docker build -t yourregistry.azurecr.io/schoollms:latest .
   docker push yourregistry.azurecr.io/schoollms:latest
   ```

2. **Deploy Container**
   ```bash
   az container create \
     --resource-group SchoolLMS-RG \
     --name schoollms-container \
     --image yourregistry.azurecr.io/schoollms:latest \
     --cpu 2 \
     --memory 4 \
     --ports 80 443 \
     --environment-variables \
       ASPNETCORE_ENVIRONMENT=Production
   ```

## Database Configuration

### SQLite (Default)

1. **File Location**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=./SchoolManagement.db"
     }
   }
   ```

2. **Backup Strategy**
   ```bash
   # Create backup
   cp SchoolManagement.db SchoolManagement_backup_$(date +%Y%m%d_%H%M%S).db

   # Restore backup
   cp SchoolManagement_backup_20240927_120000.db SchoolManagement.db
   ```

### SQL Server

1. **Connection String**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=SchoolManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true;Encrypt=false"
     }
   }
   ```

2. **Migration Command**
   ```bash
   dotnet ef database update --connection "Server=localhost;Database=SchoolManagementDB;Trusted_Connection=true;"
   ```

### PostgreSQL

1. **Install Provider**
   ```bash
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
   ```

2. **Update Program.cs**
   ```csharp
   builder.Services.AddDbContext<SchoolContext>(options =>
       options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```

3. **Connection String**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=schoollms;Username=postgres;Password=yourpassword"
     }
   }
   ```

## Security Configuration

### HTTPS Configuration

1. **Development Certificate**
   ```bash
   dotnet dev-certs https --trust
   ```

2. **Production Certificate**
   ```csharp
   // In Program.cs
   builder.Services.Configure<HttpsRedirectionOptions>(options =>
   {
       options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
       options.HttpsPort = 443;
   });
   ```

### Authentication Configuration

1. **Cookie Settings**
   ```csharp
   builder.Services.ConfigureApplicationCookie(options =>
   {
       options.Cookie.HttpOnly = true;
       options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
       options.LoginPath = "/Account/Login";
       options.LogoutPath = "/Account/Logout";
       options.Cookie.SameSite = SameSiteMode.Strict;
       options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
   });
   ```

2. **Data Protection**
   ```csharp
   builder.Services.AddDataProtection()
       .PersistKeysToFileSystem(new DirectoryInfo(@"./keys/"))
       .SetApplicationName("SchoolManagementSystem");
   ```

## Environment Configuration

### Development Environment

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=./Development.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Staging Environment

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=./Staging.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Production Environment

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=SchoolLMS;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft.AspNetCore": "Error"
    }
  }
}
```

## Monitoring and Logging

### Application Insights (Azure)

1. **Install Package**
   ```bash
   dotnet add package Microsoft.ApplicationInsights.AspNetCore
   ```

2. **Configure Services**
   ```csharp
   builder.Services.AddApplicationInsightsTelemetry();
   ```

### Serilog Configuration

1. **Install Packages**
   ```bash
   dotnet add package Serilog.AspNetCore
   dotnet add package Serilog.Sinks.File
   ```

2. **Configure Logging**
   ```csharp
   builder.Host.UseSerilog((context, configuration) =>
       configuration.ReadFrom.Configuration(context.Configuration));
   ```

### Health Checks

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<SchoolContext>();

app.MapHealthChecks("/health");
```

## Performance Optimization

### Caching Configuration

```csharp
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

app.UseResponseCaching();
```

### Database Optimization

1. **Connection Pooling**
   ```csharp
   builder.Services.AddDbContextPool<SchoolContext>(options =>
       options.UseSqlite(connectionString));
   ```

2. **Query Optimization**
   - Enable query compilation caching
   - Use proper indexing
   - Implement pagination for large datasets

### Static File Optimization

```csharp
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
    }
});
```

## Backup and Recovery

### Automated Backup Script (Windows)

```powershell
# BackupScript.ps1
$backupPath = "C:\Backups\SchoolLMS"
$dbPath = "C:\inetpub\wwwroot\SchoolLMS\SchoolManagement.db"
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"

if (!(Test-Path $backupPath)) {
    New-Item -ItemType Directory -Path $backupPath
}

Copy-Item $dbPath "$backupPath\SchoolManagement_$timestamp.db"

# Keep only last 30 backups
Get-ChildItem $backupPath -Name "SchoolManagement_*.db" | 
    Sort-Object | Select-Object -SkipLast 30 | 
    ForEach-Object { Remove-Item "$backupPath\$_" }
```

### Automated Backup Script (Linux)

```bash
#!/bin/bash
# backup.sh
BACKUP_DIR="/var/backups/schoollms"
DB_PATH="/var/www/schoollms/SchoolManagement.db"
TIMESTAMP=$(date +"%Y%m%d_%H%M%S")

mkdir -p $BACKUP_DIR
cp $DB_PATH "$BACKUP_DIR/SchoolManagement_$TIMESTAMP.db"

# Keep only last 30 backups
ls -t $BACKUP_DIR/SchoolManagement_*.db | tail -n +31 | xargs -d '\n' rm -f
```

### Crontab Configuration

```bash
# Run backup daily at 2 AM
0 2 * * * /path/to/backup.sh
```

## Troubleshooting

### Common Deployment Issues

1. **Port Binding Errors**
   ```bash
   # Check port usage
   netstat -tulpn | grep :80
   
   # Kill process using port
   sudo kill -9 <PID>
   ```

2. **Permission Issues**
   ```bash
   # Fix file permissions
   sudo chown -R www-data:www-data /var/www/schoollms
   sudo chmod -R 755 /var/www/schoollms
   ```

3. **Database Connection Errors**
   - Verify connection string
   - Check database server status
   - Validate credentials
   - Test network connectivity

### Performance Issues

1. **Memory Usage**
   ```bash
   # Monitor memory usage
   free -m
   htop
   ```

2. **Database Performance**
   ```sql
   -- Check slow queries
   PRAGMA optimize;
   
   -- Analyze database
   ANALYZE;
   ```

### Rollback Procedures

1. **Application Rollback**
   ```bash
   # Stop application
   sudo systemctl stop schoollms
   
   # Restore previous version
   cp -r /var/www/schoollms_backup/* /var/www/schoollms/
   
   # Start application
   sudo systemctl start schoollms
   ```

2. **Database Rollback**
   ```bash
   # Stop application
   sudo systemctl stop schoollms
   
   # Restore database backup
   cp /var/backups/schoollms/SchoolManagement_20240927_120000.db /var/www/schoollms/SchoolManagement.db
   
   # Start application
   sudo systemctl start schoollms
   ```

## Maintenance

### Regular Maintenance Tasks

1. **Weekly Tasks**
   - Review system logs
   - Check backup integrity
   - Monitor disk space usage
   - Review performance metrics

2. **Monthly Tasks**
   - Update system dependencies
   - Archive old data
   - Review user access permissions
   - Performance tuning

3. **Quarterly Tasks**
   - Security audit
   - Disaster recovery testing
   - System capacity planning
   - User training updates

### Update Procedures

1. **Application Updates**
   ```bash
   # Create backup
   ./backup.sh
   
   # Stop application
   sudo systemctl stop schoollms
   
   # Deploy new version
   dotnet publish -c Release -o /var/www/schoollms_new
   
   # Switch to new version
   mv /var/www/schoollms /var/www/schoollms_old
   mv /var/www/schoollms_new /var/www/schoollms
   
   # Run database migrations
   cd /var/www/schoollms
   dotnet SchoolManagementSystem.dll --migrate
   
   # Start application
   sudo systemctl start schoollms
   
   # Verify deployment
   curl -I http://localhost/health
   ```

## Support and Documentation

### Getting Help

- **Technical Support**: support@ugandaschoollms.com
- **Documentation**: https://docs.ugandaschoollms.com
- **Community Forum**: https://community.ugandaschoollms.com
- **GitHub Issues**: https://github.com/your-repo/issues

### Additional Resources

- **Deployment Checklist**: See checklist.md
- **Security Guidelines**: See security.md
- **Performance Tuning**: See performance.md
- **Backup Procedures**: See backup.md

---

This deployment guide is regularly updated. Please check for the latest version before deployment.