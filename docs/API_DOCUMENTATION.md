# API Documentation

## Overview
This document provides detailed information about the Uganda Curriculum School Management System's API endpoints and data structures.

## Base URL
```
https://localhost:5001/api
```

## Authentication
Currently using cookie-based authentication. Future versions will include JWT tokens.

## Controllers and Endpoints

### 1. Students Controller

#### GET /Students
Returns a list of all students with pagination.

**Parameters:**
- `page` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10)
- `search` (optional): Search term for filtering

**Response:**
```json
{
  "students": [
    {
      "id": 1,
      "fullName": "John Doe",
      "studentId": "STU001",
      "email": "john.doe@example.com",
      "level": "OLevel",
      "stream": "Science",
      "enrollmentDate": "2024-01-15T00:00:00Z"
    }
  ],
  "totalCount": 50,
  "currentPage": 1,
  "totalPages": 5
}
```

#### GET /Students/{id}
Returns detailed information about a specific student.

**Parameters:**
- `id`: Student ID

**Response:**
```json
{
  "id": 1,
  "fullName": "John Doe",
  "studentId": "STU001",
  "email": "john.doe@example.com",
  "phoneNumber": "+256700123456",
  "dateOfBirth": "2005-03-15T00:00:00Z",
  "level": "OLevel",
  "stream": "Science",
  "enrollmentDate": "2024-01-15T00:00:00Z",
  "courses": [
    {
      "id": 1,
      "courseName": "Mathematics S4",
      "subject": {
        "name": "Mathematics",
        "level": "OLevel"
      }
    }
  ]
}
```

#### POST /Students
Creates a new student.

**Request Body:**
```json
{
  "fullName": "Jane Smith",
  "studentId": "STU002",
  "email": "jane.smith@example.com",
  "phoneNumber": "+256700123457",
  "dateOfBirth": "2005-08-20T00:00:00Z",
  "level": "OLevel",
  "stream": "Arts"
}
```

### 2. Teachers Controller

#### GET /Teachers
Returns a list of all teachers.

**Response:**
```json
{
  "teachers": [
    {
      "id": 1,
      "fullName": "Dr. Mary Johnson",
      "email": "mary.johnson@school.edu.ug",
      "phoneNumber": "+256700987654",
      "qualification": "PhD Mathematics Education",
      "experience": 15,
      "subjects": ["Mathematics", "Physics"]
    }
  ]
}
```

### 3. Grading Controller

#### GET /Grading/Student/{studentId}
Returns grading information for a specific student.

**Response:**
```json
{
  "studentId": 1,
  "studentName": "John Doe",
  "level": "OLevel",
  "performances": [
    {
      "subjectName": "Mathematics",
      "assessmentType": "UCE",
      "marks": 85,
      "grade": "B",
      "gradePoint": 3.5,
      "isPassingGrade": true
    }
  ],
  "overallGPA": 3.2,
  "resultStatus": "Result1_Qualified"
}
```

#### POST /Grading/EditGrade
Updates or creates a grade for a student.

**Request Body:**
```json
{
  "studentId": 1,
  "subjectId": 2,
  "assessmentType": "UCE",
  "marks": 88,
  "term": "Term3",
  "academicYear": "2024"
}
```

### 4. Attendance Controller

#### POST /Attendance/Take
Records attendance for a course.

**Request Body:**
```json
{
  "courseId": 1,
  "attendanceDate": "2024-09-27T00:00:00Z",
  "attendances": [
    {
      "studentId": 1,
      "status": "Present",
      "checkInTime": "08:00:00",
      "notes": "On time"
    }
  ]
}
```

#### GET /Attendance/Student/{studentId}
Returns attendance records for a student.

**Parameters:**
- `startDate` (optional): Filter start date
- `endDate` (optional): Filter end date
- `courseId` (optional): Filter by course

### 5. Subjects Controller

#### GET /Subjects
Returns all subjects categorized by level and stream.

**Response:**
```json
{
  "oLevelSubjects": [
    {
      "id": 1,
      "name": "Mathematics",
      "level": "OLevel",
      "stream": "Both",
      "isCompulsory": true,
      "category": "Core"
    }
  ],
  "aLevelSubjects": [
    {
      "id": 15,
      "name": "Advanced Mathematics",
      "level": "ALevel",
      "stream": "Science",
      "isCompulsory": false,
      "category": "Principal"
    }
  ]
}
```

## Data Models

### Student Model
```csharp
public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string StudentId { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public EducationLevel Level { get; set; }
    public Stream Stream { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public virtual ICollection<CourseEnrollment> Enrollments { get; set; }
}
```

### GradeScale Model
```csharp
public class GradeScale
{
    public int Id { get; set; }
    public string Grade { get; set; }
    public int MinMark { get; set; }
    public int MaxMark { get; set; }
    public EducationLevel Level { get; set; }
    public decimal GradePoint { get; set; }
    public bool IsPassingGrade { get; set; }
}
```

### Subject Model
```csharp
public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EducationLevel Level { get; set; }
    public Stream Stream { get; set; }
    public bool IsCompulsory { get; set; }
    public SubjectCategory Category { get; set; }
}
```

## Enums

### EducationLevel
```csharp
public enum EducationLevel
{
    OLevel,    // S1-S4
    ALevel     // S5-S6
}
```

### Stream
```csharp
public enum Stream
{
    Science,
    Arts,
    Business,
    Technical,
    Both       // For subjects available in multiple streams
}
```

### AssessmentType
```csharp
public enum AssessmentType
{
    UCE,              // Uganda Certificate of Education (O-Level)
    UACE,             // Uganda Advanced Certificate of Education (A-Level)
    ContinuousAssessment,
    MidTerm,
    FinalExam,
    Practical,
    Coursework
}
```

## Error Handling

All API endpoints return standard HTTP status codes:

- `200 OK`: Successful request
- `201 Created`: Resource created successfully
- `400 Bad Request`: Invalid request data
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

Error responses follow this format:
```json
{
  "error": {
    "code": "STUDENT_NOT_FOUND",
    "message": "Student with ID 123 was not found",
    "details": "The requested student does not exist in the system"
  }
}
```

## Rate Limiting

Currently no rate limiting is implemented. Future versions will include:
- 1000 requests per hour per IP
- 500 requests per hour for authenticated users
- Special limits for bulk operations

## Versioning

API versioning is handled through URL path:
- `/api/v1/` - Current version
- `/api/v2/` - Future version

## Authentication & Authorization

### Current Implementation
- Cookie-based authentication
- Role-based access control (Admin, Teacher, Student)

### Future Implementation
- JWT token authentication
- OAuth 2.0 integration
- API key authentication for external systems

## Uganda Curriculum Specifics

### Grading Calculation
The system automatically calculates grades based on Uganda standards:

**O-Level Grading:**
- A: 90-100% (Exceptional)
- B: 80-89% (Outstanding)
- C: 70-79% (Satisfactory)
- D: 60-69% (Basic)
- E: 0-59% (Elementary)

**A-Level Grading:**
- A: 80-100% (Distinction)
- B: 70-79% (Credit)
- C: 60-69% (Pass)
- D: 50-59% (Weak Pass)
- E: 40-49% (Fail)
- O: 30-39% (Subsidiary Pass)
- F: 0-29% (Fail)

### Subject Classification
Subjects are classified according to NCDC guidelines:
- **Compulsory**: Required for all students
- **Optional**: Student can choose based on stream
- **Principal**: Main A-Level subjects
- **Subsidiary**: Supporting A-Level subjects

## Examples

### Complete Student Registration Flow
```javascript
// 1. Create student
const student = await fetch('/api/Students', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    fullName: 'John Doe',
    studentId: 'STU001',
    email: 'john@example.com',
    level: 'OLevel',
    stream: 'Science'
  })
});

// 2. Enroll in courses
const enrollment = await fetch('/api/Courses/Enroll', {
  method: 'POST',
  body: JSON.stringify({
    studentId: student.id,
    courseIds: [1, 2, 3, 4, 5]
  })
});
```

### Bulk Grade Entry
```javascript
const grades = await fetch('/api/Grading/BulkEntry', {
  method: 'POST',
  body: JSON.stringify({
    courseId: 1,
    assessmentType: 'UCE',
    grades: [
      { studentId: 1, marks: 85 },
      { studentId: 2, marks: 92 },
      { studentId: 3, marks: 78 }
    ]
  })
});
```

## Best Practices

1. **Always validate input data** before sending to the API
2. **Handle errors gracefully** with user-friendly messages
3. **Use pagination** for large datasets
4. **Cache frequently accessed data** like subjects and grade scales
5. **Follow Uganda curriculum standards** when implementing custom features
6. **Test with real Uganda educational data** to ensure compliance