# KMCSCI3110 Car Rental Application

A simple family-owned car rental web app built with ASP.NET Core MVC.


## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)  
- SQL Server (LocalDB or full instance)  
- Visual Studio 2022 (or later) / Visual Studio Code  


## Installation & Setup

1. **Clone the repository**  

Apply database migrations
In Package Manager Console (Visual Studio) or terminal:

- Update-Database

2. **Create the Admin user**

- Run the app and Login to the admin account at /Identity/Account/Login using:
- Email: admin@kmcars.net
- Password: Password1
- This user is seeded into program.cs, so only admin@kmcars.net can access admin pages.

3. **Create a normal user**

- Run the app and register a new account at /Identity/Account/Register
- Email: (your choice)
- Password: (your choice)
- This user should not have access to any admin pages.


## Running the App

1. **Admin: Vehicle Feature Management**

- Click Admin in the navbar, then select Feature Management.
- NOTE: You must be logged into admin@kmcars.net
- Click Add New Feature
- Enter the name for your vehicle feature (Sunroof, Leather, Dual Zone A/C, etc.)
- NOTE: Enter one feature at a time

3. **Admin: Vehicle Management**
   
- Click Admin in the navbar, then select Vehicle Management.
- NOTE: You must be logged into admin@kmcars.net
- Click Create New Vehicle for each car you want to add:
- NOTE: Image URL: must match a file in wwwroot/Vehicles (e.g. 2013Altima.png).
- Provide all required details (make, model, year, size, class, etc.).
- Create at least two vehicles with different sizes and classes.
- Pictures for vehicles are located at wwwroot/Vehicles/

3. **Customer: Browse & Filter Vehicles**

- Click Vehicles in the navbar.
- Click Toggle Filter to reveal filter options:
- Price range, size, class, passenger capacity, gearbox, cargo volume, etc.
- Adjust filters and sort order to narrow your search.

4. **Vehicle Details & Contact**

- Click on any vehicle card to go to Home/Details.
- On the Details page you’ll see:
- Full vehicle specs
- Contact Us form (for purchase inquiries)
- Payment Estimator

5. **Making a Reservation**

- From Details, click Reserve.
- On Home/Reserve, enter:
- Start date
- End date
- Age bracket
- The estimated cost appears. Click Confirm Reservation.
- On Home/Checkout, enter test payment info:
- Card Number: 4242 4242 4242 4242
- Expiration: 1234
- CVV: 123
- Name: John Doe
- Submission confirms the reservation.

6. **Verifying & Cancelling Reservations**

(Customer Side)
- Click Vehicles — your reserved car should no longer appear (reset filters if needed).
- Click Profile (next to Logout) to view and Cancel your reservation.

(Admin side)
- Click Admin > Reservations to see all bookings.
- Click Delete on any reservation to remove it and free up the vehicle.

7. **Business Inquiries**
   
- On any Details page, submit the Contact Us form.
- As Admin, go to Admin > Business Inquiries to review submitted messages.

8. **Statistics Dashboard**

- Click Home > Statistics to view:
- Total Vehicles
- Available Vehicles
- Unavailable Vehicles
- Average Rental Cost
- Total Accounts
- Total Reservations
- Total Revenue
- Business Inquiries
