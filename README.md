
- # Artix - Artwork Sharing Platform
  - ### Logo
    ![Artix Logo](images/icon_demo.png)
- # **Project Synopsis :**
  - **Class:** SWP391-SE1705-SP2024
  - **Project Supervisor:** Nguyen The Hoang - HoangNT20
  - **Topic:** Topic 10 (Artix)
  - **Authors:**
    - Nguyễn Dương Gia Bảo - SE171697 - Rous1141 - FE | BE | PM
    - Huỳnh Thiện Nhân - SE171117 - Dacoband - BE | Database Desgin
    - Trần Ánh Tuyết - SE170234 - GUI Design | FE
    - Nguyễn Minh Thức - SE170592 - Database Design | BE
    - Phạm Minh Triều - SE172937 - FE | Documentation
  - **Project:** Artworks Sharing and Selling Platform - Social Media, E-Commerce hybrid - Consumer to Consumer
  - **Techs:** RestfulAPI - mySQL - ReactJS - MaterializeCSS - Bootstrap - JavaScript - Paypal - MUI - .NET 8 with C# - Vercel - Azure - Avien 
  - **Structure:** 
    - Back-End:  Using MVC Structure, Database-First
    - Front-End:  
- # **Table Of Contents**
  - ### [Specifications](#specifications)
    - [Process 1](#process-1-Register-An-Account)
    - [Process 2](#process-2-check-login-information)
    - [Process 3](#process-3-product-management)
    - [Process 4](#process-4-get-product-details)
    - [Process 5](#process-5-cart-management)
    - [Process 6](#process-6-payment-management)
  - ### Technologies
  - ### Use Case Diagram
    - ### **Diagram:**
      ![Diagram](UsecaseDiagram/ArtixD.png)
  - ### Design Figma - Draw GUIs
    - ### **Login & Sign Up :**
      - Login Page :
        ![LoginPage](UI/LoginPage.png)
      - Sign Up Page :
        ![SignUpPage](UI/SignUpPage.png)
    - ### **Admin Page :**
      - *Manage : The Admin page will be for accounts with the Admin role and the Admin will manage all parameters about the Artwork, User, and Order of the User, along with a statistics table of the number of visits of the Artwork.*
        - Statistics :
          ![StatisticsPage](UI/AdminOveriew.png)
        - List Of Users : 
          ![ListOfUser](UI/AdminListOfUser.png)
        - List Of Report :
          ![ListOfReport](UI/AdminListReport.png)
        - Manage Orders :
          ![ViewReportsPage](UI/AdminManagerOrder.png)
    - ### **Users Page :**
      - *Home Page LightMode* :
        ![HomePage](UI/LightHomePage.png)
      - *Home Page DarkMode* :
        ![HomePage](UI/DarkHomePage.png)
        - *Home Page Nav-Bar * :
        ![HomePage](UI/NavBar2.png)
        ![HomePage](UI/navBar3.png)
      - *Upgrade to Premium* :
        ![UpgradeToPremiumPage](UI/AccountPackage.png)
      - *My Profile* :
        ![MyProfilePage](UI/ProfileUserPage.png)
  - ### Future Advancement
  - ### Limitation

- # Specifications
   - ### Process 1 Register An Account :
      - Send form to Guest to register
      - Phone Number
      - Email
      - Password
      - Conform password
      - Automatically assign roleid to user
   - ## Process 2 Check Login Information :
      - Get user information
      - Check whether it is a guest or a user to grant permissions to the user
      - If you are a guest, you do not have the right to buy, sell or post works on the website
      - User or Admin has the right to create products, add, remove, delete... (Only admin has the right to edit User rights.)
    - ## Process 3 Artwork Management
      - Artists will be responsible for the management of their Artwork

      - Name

      - Description

      - Tag

      - Purchasable

      - Price (If Artwork unpurchasable, artist can not set price)

    - ## Process 4 Get Product Details

      - The website sends detailed information about the work to customers in a new website

      - Name

      - Information

      - The Review of the work

    - ## Process 5 Cart Management

      - Create a cart if the cart is empty

      - Add to cart (detail to cart)

      - Name

      - Delete 1

      - Update the cart when the user makes adjustments

      - Delete the entire cart

      - Go to payment

    - ## Process 6 Payment Management

      - The store allows customers to choose payment methods in many ways:

      - Directly text the artist: Message the artist directly to make payment

      - Indirect: Online payment via banking app or apps like MOMO, ZaloPay, etc.

      - Apply discount code

      - Check whether the payment is successful or not sent by the web side that accepts the payment (if successful, go back to the payment details page and show a successful payment popup to the customer/ if it fails, go back to the cart page and show payment failed)
