# Uganda Curriculum School Management System

A comprehensive School Management System built with ASP.NET Core, specifically designed for Ugandan educational institutions following the new Competency-Based Curriculum (CBC) for O-Level and the traditional UACE system for A-Level.

## 🎯 Project Overview

This system provides complete management of students, teachers, subjects, courses, attendance, and grading according to the Uganda National Examinations Board (UNEB) standards and the National Curriculum Development Centre (NCDC) guidelines.

### Key Features

- **Uganda Curriculum Compliance**: Full support for O-Level (S1-S4) and A-Level (S5-S6) systems
- **Professional Grading System**: Implements official Uganda grading scales (A-E for both levels)
- **Comprehensive Report Cards**: Print-ready report cards with proper Uganda formatting
- **Attendance Management**: Complete attendance tracking and reporting
- **Subject Streams**: Proper categorization of Science, Arts, Business, and Technical subjects
- **Multi-Assessment Support**: UCE, UACE, Continuous Assessment, and custom assessments

## 🏗️ System Architecture

### Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: SQLite with Entity Framework Core
- **Frontend**: Razor Pages with Bootstrap 5
- **Icons**: Font Awesome 6
- **Charts**: Chart.js for analytics
- **Styling**: Custom CSS with Uganda education themes

### Project Structure

```
SchoolManagementSystem/
├── Controllers/           # MVC Controllers
│   ├── HomeController.cs
│   ├── StudentsController.cs
│   ├── TeachersController.cs
│   ├── CoursesController.cs
│   ├── GradingController.cs
│   ├── AttendanceController.cs
│   └── SubjectsController.cs
├── Models/               # Data Models
│   ├── Student.cs
│   ├── Teacher.cs
│   ├── Course.cs
│   ├── Subject.cs
│   ├── GradeScale.cs
│   ├── StudentSubjectPerformance.cs
│   ├── Attendance.cs
│   └── TeacherSubject.cs
├── Views/                # Razor Views
│   ├── Home/
│   ├── Students/
│   ├── Teachers/
│   ├── Courses/
│   ├── Grading/
│   ├── Attendance/
│   ├── Subjects/
│   └── Shared/
├── Data/                 # Database Context
│   └── SchoolContext.cs
├── Migrations/           # EF Core Migrations
└── wwwroot/             # Static Files
    ├── css/
    ├── js/
    └── lib/
```

## 📚 Uganda Curriculum Implementation

### O-Level (Lower Secondary - S1 to S4)

#### Grading System (New CBC)
| Grade | Range | Description | Status |
|-------|-------|-------------|---------|
| A | 90-100% | Exceptional | Pass |
| B | 80-89% | Outstanding | Pass |
| C | 70-79% | Satisfactory | Pass |
| D | 60-69% | Basic | Pass |
| E | 0-59% | Elementary | Fail |

#### Result Classifications
- **Result 1**: Qualifies for UCE certificate (meets all requirements)
- **Result 2**: Does not qualify (missing compulsory subjects/assessments)
- **Result 3**: Below basic competency (all E grades)

#### Compulsory Subjects
- English Language
- Mathematics
- Physics
- Chemistry
- Biology
- History & Political Education
- Geography

#### Optional Subjects
- Literature in English
- Local Languages (Luganda, Runyankole, etc.)
- Kiswahili
- Christian Religious Education (CRE)
- Islamic Religious Education (IRE)
- Agriculture
- ICT/Computer Studies
- Entrepreneurship
- Fine Art
- Physical Education
- Technology & Design
- Nutrition & Food Technology

### A-Level (Upper Secondary - S5 to S6)

#### Grading System (UACE)
| Grade | Range | Description | Status |
|-------|-------|-------------|---------|
| A | 80-100% | Distinction | Pass |
| B | 70-79% | Credit | Pass |
| C | 60-69% | Pass | Pass |
| D | 50-59% | Weak Pass | Pass |
| E | 40-49% | Fail | Fail |
| O | 30-39% | Subsidiary Pass | Subsidiary |
| F | 0-29% | Fail | Fail |

#### Subject Combinations

**Science Stream:**
- Mathematics
- Physics
- Chemistry
- Biology
- Subsidiary Mathematics
- Subsidiary ICT

**Arts Stream:**
- History & Political Education
- Geography
- Literature in English
- CRE/IRE
- Economics
- Fine Art

**Business Stream:**
- Economics
- Geography
- Mathematics
- Entrepreneurship
- Subsidiary subjects

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQLite (included with .NET)

### Installation

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd SchoolManagementSystem
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Update database**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Open browser to `https://localhost:5001` or `http://localhost:5000`

### First Run Setup

The system automatically seeds with:
- Uganda grading scales for both O-Level and A-Level
- Complete subject list (27 subjects) categorized by level and stream
- Sample data for testing

## 📊 Core Modules

### 1. Student Management
- **Registration**: Complete student profiles with Uganda-specific fields
- **Academic Tracking**: Education level, stream, academic year
- **Performance Analytics**: GPA calculation, progress tracking
- **Report Generation**: Professional Uganda-format report cards

### 2. Teacher Management
- **Professional Profiles**: Qualifications, experience, specializations
- **Subject Assignments**: Link teachers to subjects they can teach
- **Performance Tracking**: Teaching load, student performance metrics

### 3. Subject Management
- **Curriculum Compliance**: Official Uganda subjects with proper categorization
- **Stream Classification**: Science, Arts, Business, Technical streams
- **Level Management**: O-Level and A-Level distinction
- **Teacher Assignment**: Subject-teacher relationships

### 4. Grading System
- **Multi-Assessment Support**: 
  - Uganda Certificate of Education (UCE)
  - Uganda Advanced Certificate of Education (UACE)
  - Continuous Assessment
  - Mid-term Exams
  - Final Exams
- **Grade Calculation**: Automatic letter grade assignment
- **Performance Analytics**: Class statistics, grade distribution
- **Report Cards**: Print-ready with official formatting

### 5. Attendance Management
- **Daily Attendance**: Mark present/absent/late for students
- **Course-Specific**: Track attendance per subject
- **Analytics**: Attendance trends, patterns, alerts
- **Reports**: Individual and class attendance summaries

### 6. Course Management
- **Course Creation**: Link subjects to specific terms/classes
- **Enrollment**: Student-course registration
- **Capacity Management**: Maximum enrollment limits
- **Schedule Integration**: Time slots and teacher assignments

## 🎨 User Interface

### Dashboard Features
- **Real-time Statistics**: Student, teacher, course counts
- **Performance Metrics**: Grade distributions, pass rates
- **Attendance Overview**: Daily attendance summaries
- **Quick Actions**: Common tasks and navigation

### Design Principles
- **Uganda Education Theme**: Colors and styling appropriate for educational institutions
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **Accessibility**: WCAG compliant with proper contrast and navigation
- **Print-Friendly**: Report cards and documents optimized for printing

## 📈 Reports and Analytics

### Academic Reports
- **Individual Report Cards**: Complete student performance with Uganda grading
- **Class Performance**: Subject-wise analytics and comparisons
- **Progress Tracking**: Term-over-term performance trends
- **Grade Distribution**: Statistical analysis of class performance

### Attendance Reports
- **Individual Attendance**: Student-specific attendance history
- **Class Attendance**: Daily, weekly, monthly summaries
- **Subject Attendance**: Course-specific attendance tracking
- **Trend Analysis**: Attendance patterns and alerts

### Administrative Reports
- **Enrollment Statistics**: Student distribution by level and stream
- **Teacher Workload**: Subject assignments and teaching loads
- **Performance Analytics**: School-wide academic performance
- **Custom Reports**: Flexible reporting with date ranges and filters

## 🔧 Configuration

### Database Configuration
The system uses SQLite by default. To change to SQL Server or other databases:

1. Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SchoolDB;Trusted_Connection=true;"
  }
}
```

2. Update `Program.cs`:
```csharp
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(connectionString));
```

### Grading Configuration
Grading scales are configurable through the database. To modify:

1. Access the `GradeScales` table
2. Update grade ranges and descriptions
3. Restart the application to refresh cache

### Subject Configuration
Subjects can be added or modified through the admin interface or database:

1. Navigate to **Subjects > Create New**
2. Specify level (O-Level/A-Level), stream, and type
3. Assign qualified teachers

## 🧪 Testing

### Running Tests
```bash
dotnet test
```

### Sample Data
The system includes comprehensive seed data for testing:
- 50+ sample students across different levels
- 20+ teachers with various specializations
- Complete subject curriculum
- Sample grades and attendance records

## 🚀 Deployment

### Local Deployment
1. Build the project: `dotnet build --configuration Release`
2. Publish: `dotnet publish --configuration Release`
3. Run: `dotnet SchoolManagementSystem.dll`

### Cloud Deployment (Azure)
1. Create Azure Web App
2. Configure connection strings
3. Deploy using Visual Studio or Azure CLI
4. Update database connection for production

### Docker Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "SchoolManagementSystem.dll"]
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-feature`)
3. Commit changes (`git commit -am 'Add new feature'`)
4. Push to branch (`git push origin feature/new-feature`)
5. Create Pull Request

### Coding Standards
- Follow C# naming conventions
- Use meaningful variable and method names
- Add XML documentation for public methods
- Follow SOLID principles
- Write unit tests for new features

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For support and questions:
- **Email**: support@ugandaschoollms.com
- **Documentation**: [Project Wiki](docs/)
- **Issues**: [GitHub Issues](https://github.com/your-repo/issues)

## 🙏 Acknowledgments

- **Uganda National Examinations Board (UNEB)** for grading standards
- **National Curriculum Development Centre (NCDC)** for curriculum guidelines
- **Ministry of Education and Sports, Uganda** for educational policies
- **Bootstrap Team** for the UI framework
- **Microsoft** for ASP.NET Core framework

## 📋 Changelog

### Version 1.0.0 (Current)
- ✅ Complete Uganda curriculum implementation
- ✅ Professional grading system
- ✅ Comprehensive attendance management
- ✅ Report card generation
- ✅ Multi-user role system
- ✅ Responsive web interface

### Planned Features (Version 1.1.0)
- 🔄 SMS notifications for parents
- 🔄 Mobile app for teachers
- 🔄 Advanced analytics dashboard
- 🔄 Export to Excel/PDF features
- 🔄 Multi-school support
- 🔄 Payment integration for fees

---

**Built with ❤️ for Uganda's educational future**