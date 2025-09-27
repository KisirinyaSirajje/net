# Contributing to Uganda Curriculum School Management System

We welcome contributions to the Uganda Curriculum School Management System! This document provides guidelines for contributing to the project.

## Table of Contents

1. [Code of Conduct](#code-of-conduct)
2. [Getting Started](#getting-started)
3. [How to Contribute](#how-to-contribute)
4. [Development Setup](#development-setup)
5. [Coding Standards](#coding-standards)
6. [Testing Guidelines](#testing-guidelines)
7. [Pull Request Process](#pull-request-process)
8. [Issue Reporting](#issue-reporting)
9. [Uganda Curriculum Guidelines](#uganda-curriculum-guidelines)

## Code of Conduct

### Our Pledge

We pledge to make participation in our project a harassment-free experience for everyone, regardless of age, body size, disability, ethnicity, gender identity and expression, level of experience, nationality, personal appearance, race, religion, or sexual identity and orientation.

### Expected Behavior

- Use welcoming and inclusive language
- Be respectful of differing viewpoints and experiences
- Gracefully accept constructive criticism
- Focus on what is best for the community and Uganda's education system
- Show empathy towards other community members

### Unacceptable Behavior

- Harassment of any kind
- Discriminatory language or behavior
- Personal attacks or trolling
- Publishing others' private information without permission
- Other conduct which could reasonably be considered inappropriate

## Getting Started

### Prerequisites

Before contributing, ensure you have:

1. **.NET 8.0 SDK** installed
2. **Git** for version control
3. **Visual Studio 2022** or **VS Code** (recommended)
4. Basic understanding of:
   - ASP.NET Core MVC
   - Entity Framework Core
   - Uganda education system (O-Level and A-Level)
   - HTML, CSS, JavaScript

### First Time Setup

1. **Fork the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/uganda-school-lms.git
   cd uganda-school-lms
   ```

2. **Set up upstream remote**
   ```bash
   git remote add upstream https://github.com/ORIGINAL_OWNER/uganda-school-lms.git
   ```

3. **Install dependencies**
   ```bash
   dotnet restore
   ```

4. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

## How to Contribute

### Types of Contributions We Welcome

1. **Bug Fixes**: Report and fix bugs in the system
2. **Feature Enhancements**: Add new features that benefit Uganda's education system
3. **Documentation**: Improve user guides, API documentation, or code comments
4. **Testing**: Add unit tests, integration tests, or improve test coverage
5. **Performance Improvements**: Optimize database queries, caching, or UI performance
6. **Uganda Curriculum Updates**: Keep the system aligned with UNEB and NCDC standards
7. **Accessibility Improvements**: Make the system more accessible to users with disabilities
8. **Localization**: Add support for local Ugandan languages

### Contribution Workflow

1. **Check existing issues** to avoid duplicate work
2. **Create or comment on an issue** to discuss your proposed changes
3. **Fork the repository** and create a feature branch
4. **Make your changes** following our coding standards
5. **Write or update tests** for your changes
6. **Update documentation** if necessary
7. **Submit a pull request** with a clear description

## Development Setup

### Project Structure Understanding

```
SchoolManagementSystem/
├── Controllers/         # MVC Controllers
├── Models/             # Data Models (Uganda curriculum specific)
├── Views/              # Razor Views
├── Data/               # Database Context and Migrations
├── wwwroot/           # Static files (CSS, JS, images)
├── Migrations/        # EF Core Migrations
└── docs/             # Documentation
```

### Environment Configuration

1. **Development Environment**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=Development.db"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information"
       }
     }
   }
   ```

2. **Testing Environment**
   ```bash
   dotnet test
   ```

### Database Development

When working with database changes:

1. **Create migrations**
   ```bash
   dotnet ef migrations add YourMigrationName
   ```

2. **Update database**
   ```bash
   dotnet ef database update
   ```

3. **Always include seed data** for Uganda curriculum elements

## Coding Standards

### C# Coding Conventions

Follow Microsoft's C# coding conventions:

```csharp
// Good: PascalCase for public members
public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    
    // Good: Descriptive method names
    public bool IsEligibleForPromotion()
    {
        // Implementation
    }
}

// Good: Meaningful variable names
var ugandaGradingService = new UgandaGradingService();
var oLevelGradeScales = await gradingService.GetOLevelGradeScalesAsync();
```

### Naming Conventions

- **Classes**: PascalCase (`StudentPerformance`, `UgandaGradingService`)
- **Methods**: PascalCase (`CalculateGradePointAverage`, `GetStudentsByLevel`)
- **Properties**: PascalCase (`FullName`, `EducationLevel`)
- **Variables**: camelCase (`studentId`, `gradeCalculation`)
- **Constants**: UPPER_CASE (`MAX_STUDENTS_PER_CLASS`, `UGANDA_PASSING_GRADE`)
- **Private fields**: _camelCase (`_context`, `_gradingService`)

### File Organization

```csharp
// File header with purpose
/// <summary>
/// Handles grading operations according to Uganda curriculum standards.
/// Supports both O-Level (UCE) and A-Level (UACE) grading systems.
/// </summary>
public class UgandaGradingService
{
    // Constants first
    private const int MINIMUM_PASSING_GRADE = 60;
    
    // Private fields
    private readonly SchoolContext _context;
    private readonly ILogger<UgandaGradingService> _logger;
    
    // Constructor
    public UgandaGradingService(SchoolContext context, ILogger<UgandaGradingService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    // Public methods
    public async Task<string> CalculateGradeAsync(int marks, EducationLevel level)
    {
        // Implementation
    }
    
    // Private methods
    private bool IsValidGrade(string grade)
    {
        // Implementation
    }
}
```

### Documentation Standards

Use XML documentation comments for all public members:

```csharp
/// <summary>
/// Calculates the grade based on marks and education level according to Uganda standards.
/// </summary>
/// <param name="marks">The numerical marks (0-100)</param>
/// <param name="level">The education level (O-Level or A-Level)</param>
/// <returns>The letter grade (A, B, C, D, E for O-Level; A, B, C, D, E, O, F for A-Level)</returns>
/// <exception cref="ArgumentOutOfRangeException">Thrown when marks are not between 0 and 100</exception>
public async Task<string> CalculateGradeAsync(int marks, EducationLevel level)
{
    if (marks < 0 || marks > 100)
        throw new ArgumentOutOfRangeException(nameof(marks), "Marks must be between 0 and 100");
        
    // Implementation
}
```

## Testing Guidelines

### Unit Testing

Write unit tests for all business logic:

```csharp
[TestClass]
public class UgandaGradingServiceTests
{
    private SchoolContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new SchoolContext(options);
    }

    [TestMethod]
    [DataRow(95, EducationLevel.OLevel, "A")]
    [DataRow(85, EducationLevel.OLevel, "B")]
    [DataRow(75, EducationLevel.OLevel, "C")]
    [DataRow(65, EducationLevel.OLevel, "D")]
    [DataRow(55, EducationLevel.OLevel, "E")]
    public async Task CalculateGrade_OLevel_ReturnsCorrectGrade(int marks, EducationLevel level, string expectedGrade)
    {
        // Arrange
        using var context = GetInMemoryContext();
        SeedOLevelGradeScales(context);
        var service = new UgandaGradingService(context, Mock.Of<ILogger<UgandaGradingService>>());

        // Act
        var actualGrade = await service.CalculateGradeAsync(marks, level);

        // Assert
        Assert.AreEqual(expectedGrade, actualGrade);
    }
}
```

### Integration Testing

Test complete workflows:

```csharp
[TestClass]
public class GradingControllerIntegrationTests
{
    [TestMethod]
    public async Task GenerateReportCard_ValidStudent_ReturnsReportCard()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        
        // Seed test data
        await SeedTestDataAsync(factory);

        // Act
        var response = await client.GetAsync("/Grading/ReportCard/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains("Uganda Secondary School"));
        Assert.IsTrue(content.Contains("Report Card"));
    }
}
```

### Test Coverage

Aim for:
- **80%+ code coverage** for business logic
- **60%+ code coverage** overall
- **100% coverage** for Uganda curriculum-specific calculations

Run coverage reports:
```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report
```

## Pull Request Process

### Before Creating a Pull Request

1. **Ensure your branch is up to date**
   ```bash
   git checkout main
   git pull upstream main
   git checkout your-feature-branch
   git merge main
   ```

2. **Run all tests**
   ```bash
   dotnet test
   ```

3. **Check code quality**
   ```bash
   dotnet format
   dotnet build --configuration Release
   ```

### Pull Request Template

When creating a pull request, use this template:

```markdown
## Description
Brief description of the changes made.

## Type of Change
- [ ] Bug fix (non-breaking change that fixes an issue)
- [ ] New feature (non-breaking change that adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update
- [ ] Uganda curriculum update

## Uganda Curriculum Compliance
- [ ] Follows UNEB grading standards
- [ ] Complies with NCDC subject categorization
- [ ] Supports both O-Level and A-Level systems
- [ ] Maintains backward compatibility with existing data

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] Manual testing completed
- [ ] All existing tests pass

## Checklist
- [ ] Code follows project style guidelines
- [ ] Self-review completed
- [ ] Documentation updated (if applicable)
- [ ] No merge conflicts
- [ ] Linked to related issue(s)

## Screenshots (if applicable)
Add screenshots to help explain your changes.

## Additional Notes
Any additional information about the changes.
```

### Review Process

1. **Automated checks** must pass
2. **At least one code review** required
3. **Uganda curriculum expert review** for curriculum-related changes
4. **Documentation review** for user-facing changes
5. **Final approval** from maintainer

## Issue Reporting

### Bug Reports

Use this template for bug reports:

```markdown
## Bug Description
Clear description of what the bug is.

## Steps to Reproduce
1. Go to '...'
2. Click on '...'
3. Scroll down to '...'
4. See error

## Expected Behavior
What you expected to happen.

## Actual Behavior
What actually happened.

## Environment
- OS: [e.g., Windows 10, Ubuntu 20.04]
- Browser: [e.g., Chrome 95, Firefox 93]
- .NET Version: [e.g., 8.0]
- Database: [e.g., SQLite, SQL Server]

## Uganda Curriculum Context (if applicable)
- Education Level: [O-Level, A-Level]
- Subject/Stream: [Science, Arts, Business, etc.]
- Specific curriculum requirement affected

## Screenshots
Add screenshots if applicable.

## Additional Context
Any other context about the problem.
```

### Feature Requests

Use this template for feature requests:

```markdown
## Feature Description
Clear description of the feature you'd like to see.

## Uganda Education Context
- How does this relate to Uganda's education system?
- Which UNEB/NCDC requirements does it address?
- What education levels does it affect?

## User Story
As a [role], I want [functionality] so that [benefit].

## Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2
- [ ] Criterion 3

## Implementation Suggestions
Your ideas on how to implement this feature.

## Additional Context
Any other context or screenshots about the feature request.
```

## Uganda Curriculum Guidelines

### Education System Understanding

Before contributing, understand Uganda's education structure:

1. **Primary Level**: P1-P7 (not covered by this system)
2. **O-Level (Lower Secondary)**: S1-S4, culminating in UCE
3. **A-Level (Upper Secondary)**: S5-S6, culminating in UACE

### Grading Standards

#### O-Level (UCE) Standards
- **A**: 90-100% (Exceptional)
- **B**: 80-89% (Outstanding) 
- **C**: 70-79% (Satisfactory)
- **D**: 60-69% (Basic)
- **E**: 0-59% (Elementary)

#### A-Level (UACE) Standards
- **A**: 80-100% (Distinction)
- **B**: 70-79% (Credit)
- **C**: 60-69% (Pass)
- **D**: 50-59% (Weak Pass)
- **E**: 40-49% (Fail)
- **O**: 30-39% (Subsidiary Pass)
- **F**: 0-29% (Fail)

### Subject Categories

When adding or modifying subjects, follow NCDC categorization:

#### O-Level Subjects
**Compulsory:**
- English Language
- Mathematics  
- Physics, Chemistry, Biology
- History & Political Education
- Geography

**Optional:**
- Literature in English
- Local Languages
- Religious Education
- Agriculture, ICT, etc.

#### A-Level Subjects
**Streams:**
- **Science**: Mathematics, Physics, Chemistry, Biology
- **Arts**: History, Geography, Literature, Economics
- **Business**: Economics, Entrepreneurship, Geography, Mathematics

### Cultural Sensitivity

When contributing:
- Respect Uganda's cultural diversity
- Use inclusive language
- Consider multilingual users
- Understand local educational practices
- Be sensitive to economic constraints of Ugandan schools

## Recognition

### Contributors

We recognize contributors in several ways:

1. **GitHub Contributors** page
2. **CONTRIBUTORS.md** file
3. **Release notes** acknowledgments
4. **Annual contributor awards**

### Special Recognition Categories

- **Uganda Education Expert**: Deep understanding of curriculum
- **Technical Excellence**: Outstanding code quality
- **Documentation Champion**: Exceptional documentation contributions
- **Community Builder**: Active in discussions and support

## Getting Help

### Support Channels

1. **GitHub Issues**: Technical problems and bugs
2. **Discussions**: General questions and ideas
3. **Email**: Sensitive issues (security, etc.)
4. **Community Forum**: User support and tips

### Mentorship Program

New contributors can request mentorship:
- Guidance on Uganda education system
- Technical architecture overview  
- Code review and best practices
- Career development in EdTech

## Resources

### Uganda Education Resources
- [Uganda National Examinations Board (UNEB)](https://uneb.ac.ug/)
- [National Curriculum Development Centre (NCDC)](https://ncdc.go.ug/)
- [Ministry of Education and Sports](https://education.go.ug/)

### Technical Resources
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core/)
- [C# Coding Conventions](https://docs.microsoft.com/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)

Thank you for contributing to Uganda's educational future! 🇺🇬📚