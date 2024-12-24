# Inno_Shop 
The "Inno_Shop" project is a system consisting of two microservices designed for managing users and their products. These microservices are developed using ASP.NET Core and interact via APIs. The microservices use their own databases.

## Tech stack:
* Minimal API.
* JWT (JSON Web Tokens) is used for user authentication and authorization.
* PostgreSQL is used as a database management system for storing microservice data.
* Entity Framework with the Code First approach is used for database access.
* The microservices are designed to be deployable in Docker (Docker/Docker Compose).
* The database is included as part of the Docker Compose file (along with pgAdmin).
* Fluent Validation is applied to data models validation.
* Problem Details and Exception handler are used for error handling and reporting

Start the solution in VS in ‘Docker Compose’ mode to run all the required containers. 
Microservices use data from environment variables, they described in the .env file:

```
ASPNETCORE_ENVIRONMENT=Development
JWT_SECRET_KEY=my_super_secret_key_32_bytes_long
POSTGRES_USERS_DB=innoshop_users
POSTGRES_PRODUCTS_DB=innoshop_products
POSTGRES_USER=innoshop_user
POSTGRES_PASSWORD=innoshop_password
USERS_API_BASEURL=http://users.api:8080
PRODUCTS_API_BASEURL=http://products.api:8082
X_SVC_AUTH_HEADER=8291d15b-9fa3-43a2-94ec-2a686daad548
```

# Users API
The microservice provides a RESTful API for creating, reading, updating, and deleting users.
* Each user has the following attributes: ID, name, email address, role, etc.
* The microservice implements user authentication and authorization using access tokens.
* Password recovery mechanisms are implemented.
* An email-based account confirmation system is included.
* Deactivating a user hides all their products. A SoftDelete mechanism is implemented in the product microservice to ensure that all products reappear when the user is reactivated.

Explore the Users API endpoints by this link after project starts:
```
http://localhost:8080/swagger/index.html 
Default admin user to login:
Email: admin@admin.com
Passw: admin
```

# Products API
The second microservice, "Product Management," provides a RESTful API for creating, reading, updating, and deleting products. User authentication is implemented via the Users API service.
* Each product has attributes such as ID, name, description, price, availability, the ID of the user who created the product, creation date, etc.
* The microservice includes functionality for searching and filtering products by various parameters.
* Mechanisms for error handling and data validation are implemented.
* Only authorized users can add, delete, or edit products, and these operations can only be performed on products owned by the respective user.

Explore the Users API endpoints by this link after project starts:
```
http://localhost:8082/swagger/index.html 
```

# Possible improvements
* Apply response caching
* .Net Aspire orchestration for microservices communication
* Localization (use resources)
* Advanced logging with Serilog
* Add load testing
