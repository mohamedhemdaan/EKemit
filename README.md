# EKemit Marketplace API

## Overview

EKemit is a marketplace platform designed to promote and sell local products from Egyptian governorates.

The project is built using ASP.NET Core Web API and follows Onion Architecture principles to provide a scalable, maintainable, and clean architecture solution.

The platform supports user authentication, product management, shopping baskets, order processing, online payments, ratings, comments, role management, and administration features.

---

## Features

### Authentication & Accounts

* User Registration and Login
* JWT Authentication
* Change Password
* Forgot Password & Reset Password
* Email Verification & Notifications
* User Address Management
* Current User Profile

### Administration

* User Management
* Role Management
* Assign Users to Roles
* Update User Information
* View All Users
* View All Roles

### Products & Catalog

* Product Management
* Product Types
* Product Brands
* Governorates Management
* Product Search
* Product Filtering
* Product Sorting
* Pagination

### Shopping Basket

* Create Basket
* Update Basket
* Delete Basket

### Orders & Payments

* Order Creation
* Order History
* Delivery Methods
* Stripe Payment Integration
* Stripe Webhooks

### Community Features

* Product Ratings
* Product Comments
* Contact Us Module

### Performance

* Redis Caching
* Global Exception Handling
* API Validation

---

## Architecture

The project follows Onion Architecture and is organized into the following layers:

### Core Layer

* Entities
* Interfaces
* Business Rules

### Repository Layer

* Entity Framework Core
* Repository Pattern
* Data Access Logic

### Service Layer

* Business Logic
* Authentication Services
* Payment Services
* Order Services

### API Layer

* Controllers
* Middleware
* REST Endpoints

---

## Design Patterns & Principles

* Onion Architecture
* Repository Pattern
* Unit of Work Pattern
* Specification Pattern
* Dependency Injection
* SOLID Principles

---

## Technologies Used

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT Authentication
* Redis Cache
* Stripe Payment Gateway
* AutoMapper
* MailKit
* Swagger

---

## Main Modules

* Accounts
* Administration
* Products
* Brands
* Types
* Governorates
* Basket
* Orders
* Payments
* Ratings
* Comments
* Contact Us

---

## Getting Started

1. Clone the repository.

2. Configure the database connection string.

3. Configure JWT, Email, Redis, and Stripe settings.

4. Apply migrations:

```bash
dotnet ef database update
```

5. Run the application.

```bash
dotnet run
```

---

## Notes

Sensitive information such as API keys, JWT secrets, email credentials, and Stripe keys should be stored using User Secrets or Environment Variables and should not be committed to source control.
