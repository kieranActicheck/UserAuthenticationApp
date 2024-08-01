# UserAuthenticationApp

UserAuthenticationApp is a web application that provides user authentication and management functionalities. It includes an API for retrieving user information securely using Data Transfer Objects (DTOs).

## API Endpoints
Get User by ID

•	URL: /api/user/{id}

•	Method: GET

•	Description: Retrieves user information by user ID.

•	Response: Returns a UserDto object containing user information.

## Configuration
The UserController class handles user-related API endpoints. The UserDto class is used to exclude sensitive fields from API responses.

## Contributing
Contributions are always welcome! Please follow these steps to contribute:
1.	Fork the repository.
2.	Create a new branch (git checkout -b feature-branch).
3.	Make your changes and commit them (git commit -m 'Add some feature').
4.	Push to the branch (git push origin feature-branch).
5.	Create a Pull Request.
