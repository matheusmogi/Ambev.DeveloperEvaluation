# Ambev Developer Evaluation Project

This project is a sample application designed to demonstrate the implementation of a modern, scalable, and maintainable system using technologies such as RabbitMQ, MongoDB, EF Core, and MediatR. It showcases event-driven architecture, domain-driven design principles, and clean code practices.

---

## 🛠️ Project Overview

The system handles sales transactions, allowing for the creation, cancellation, and tracking of sales. It uses RabbitMQ for message-based communication, MongoDB for event storage, and EF Core for relational data management.

---

## 🏗️ Key Technologies

### 1. **RabbitMQ** 🐇

- Used as a message broker to enable asynchronous communication between services.
- Example: When a sale is canceled, a message is published to RabbitMQ and consumed by a handler.

### 2. **MongoDB** 🍃

- Serves as a NoSQL database for storing event data, such as sale cancellations.

### 3. **EF Core** 🗄️

- Provides an ORM for managing relational data, such as sales and customers.

### 4. **MediatR** 📬

- Facilitates in-process messaging to decouple components and improve maintainability.

### 5. **MassTransit**

- Simplifies integration with RabbitMQ for message publishing and consumption.

---

## 🗂️ Project Structure

- **Domain Layer**: Contains core business logic and domain entities.
- **Application Layer**: Handles use cases and application-specific logic.
- **Infrastructure Layer**: Manages external dependencies like databases and message brokers.
- **API Layer**: Exposes RESTful endpoints for client interaction.
- **Messages**: Defines the structure of messages exchanged between components.
- **Unit Tests**: Ensures the correctness of the system through automated tests.
- **Integration Tests**: Validates the interaction between different components and services.

---

## 🚀 How to Run the Project

#### Clone the Repository.
```bash
git clone https://github.com/tibursocampos/Ambev.DeveloperEvaluation.git
cd Ambev.DeveloperEvaluation
```
### Option 1: Using Docker Compose

The project can be run entirely using `docker-compose`, which sets up RabbitMQ, MongoDB, Postgres, and the application.

#### 1. Ensure `docker-compose.yml` is in the root directory.
#### 2. Build and start the services:
   ```bash
   docker-compose up --build
   ```

### Option 2: Running in Debug Mode (Optional)

If you want to run the application in debug mode:

#### 1. Open the project in your preferred IDE.
#### 2. Select Docker Compose as the startup option.
#### 3. Run the application.

### After starting the services

#### 1. After starting the services, apply the migrations:
   ```bash
   dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi --context DefaultContext --connection "Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n;Pooling=true;"
   ```
#### 2. Access the application at `https://localhost:8082/swagger/index.html`. Use Swagger to interact with the sales endpoints for creating, updating, querying, selecting, and deleting sales.
#### 3. It is possible to see the RabbitMQ management interface at:
```bash 
http://127.0.0.1:15672/#/
```
##### Credentials for Access:

- Username: developer
- Password: ev@luAt10n

#### 4. Access MongoDB at http://localhost:27017 to check if the `sale events` are being saved by consumers observing the RabbitMQ queues.
