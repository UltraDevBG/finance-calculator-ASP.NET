# 💰 Finance Calculator (ASP.NET Core)

**Finance Calculator** is a comprehensive web application built with ASP.NET Core, designed to help users manage their personal finances, calculate loans, and plan savings effectively.

## 🚀 Features

The application provides several powerful financial modules:

*   **Loan Calculator:** Calculate monthly installments, total interest, and total repayment amounts for both annuity and decreasing payment schedules.
*   **Savings/Deposit Calculator:** Forecast investment growth with compound interest and different capitalization periods.
*   **Currency Converter:** Live currency conversion using external API integration (optional/configurable).
*   **Amortization Schedule:** Detailed breakdown of each payment over the loan term.
*   **Data Visualization:** Interactive charts and graphs to visualize debt vs. principal and savings growth.

## 🛠️ Tech Stack

*   **Backend:** [ASP.NET Core 6.0/7.0+](https://dotnet.microsoft.com) (C#)
*   **Frontend:** Razor Pages / MVC, Bootstrap 5, [Chart.js](https://www.chartjs.org) for visualizations.
*   **Database:** Entity Framework Core with SQL Server (easily switchable to SQLite or PostgreSQL).
*   **Architecture:** Clean Architecture / Repository Pattern.

## 🏗️ Installation & Setup

Follow these steps to get the project running locally:

1.  **Clone the repository:**
    ```bash
    git clone https://github.com
    cd finance-calculator-ASP.NET
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Database Configuration:**
    Update the `ConnectionStrings` in `appsettings.json` if you wish to use a specific SQL Server instance.

4.  **Apply Migrations:**
    ```bash
    dotnet ef database update
    ```

5.  **Run the application:**
    ```bash
    dotnet run
    ```
    Navigate to `https://localhost:5001` or `http://localhost:5000` in your browser.

## 📂 Project Structure

*   `Controllers/` – Handles user requests and bridges the gap between Logic and View.
*   `Models/` – Data structures and validation logic for financial entities.
*   `Views/` – Responsive UI built with Razor and Bootstrap.
*   `Services/` – Core business logic and financial algorithms.
*   `wwwroot/` – Static assets (Custom CSS, JavaScript, Images).

## 📊 Mathematical Formulas Used

The engine utilizes standard financial mathematics:
*   **Annuity Payment:** $A = P \frac{r(1+r)^n}{(1+r)^n - 1}$
*   **Compound Interest:** $A = P(1 + \frac{r}{n})^{nt}$

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create.
1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

Distributed under the **MIT License**. See `LICENSE` for more information.

## 📧 Contact

**Yuliyan Tsvetanov** - [GitHub Profile](https://github.com)  
Project Link: [https://github.com/finance-calculator-ASP.NET](https://github.com/finance-calculator-ASP.NET)
