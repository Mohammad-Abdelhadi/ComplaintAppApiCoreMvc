API Documentation for Complaints Management System
Introduction
Welcome to the Complaints Management System API documentation. This API provides functionalities to manage user complaints and related operations. Below, you will find detailed information on the available endpoints, their functionalities, and request/response formats.

Authentication
Authentication is required for certain endpoints. You can authenticate by sending a POST request to the /api/Auth/login endpoint with valid credentials (email and password). Upon successful authentication, you will receive a token that should be included in the Authorization header for subsequent requests to secured endpoints.

Authentication Endpoints
POST /api/Auth/register
Registers a new user in the system.

Request Body:



{
  "Username": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "Password": "string",
  "ConfirmPassword": "string"
}
Response:



{
  "message": "Registration successful",
  "Id": 1,
  "Email": "string",
  "Username": "string",
  "phonenumber": "string",
  "role": "user"
}
POST /api/Auth/login
Logs in an existing user.

Request Body:



{
  "Email": "string",
  "Password": "string"
}
Response:



{
  "Id": 1,
  "Username": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "Password": "string",
  "Role": "string"
}
Complaint Endpoints
GET /api/Complaint/GetComplaints/{Id}
Gets all complaints for a specific user.

Parameters:

Id (int) - User ID
Response:



Returns a list of complaints for the specified user.
GET /api/Complaint/GetUserComplaints/{id}
Gets all complaints for a single user.

Parameters:

id (int) - User ID
Response:



Returns a list of complaints for the specified user.
GET /api/Complaint/GetSingleComplaint/{id}
Gets a single complaint by its ID.

Parameters:

id (int) - Complaint ID
Response:



Returns a single complaint based on the provided ID.
PUT /api/Complaint/EditComplaint/{id}
Edits a complaint's approval status.

Parameters:

id (int) - Complaint ID
Request Body:



{
  "isApproved": true
}
Response:



Empty response upon successful edit.
POST /api/Complaint/sendcomplaint
Submits a new complaint.

Request Body:



{
  "ComplaintText": "string",
  "FileName": "string",
  "Language": "string",
  "IsApproved": true,
  "UserId": 1,
  "Demands": [
    {
      "DemandText": "string"
    }
  ]
}
Response:



{
  "message": "Image uploaded successfully."
}
Error Responses
In case of errors, the API will respond with appropriate HTTP status codes and error messages in the response body.

400 Bad Request: Invalid request format or missing parameters.
401 Unauthorized: Authentication failure or insufficient permissions.
404 Not Found: Resource not found.
409 Conflict: Resource conflict, such as duplicate registration attempts.
500 Internal Server Error: Server-side error occurred.
