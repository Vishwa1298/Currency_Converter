# Currency_Converter
This is a Windows Presentation Foundation (WPF) application that allows users to convert currencies based on real-time exchange rates. It fetches exchange rate data from the Open Exchange Rates API and provides an easy-to-use interface for currency conversion.

✨ Features
Real-time exchange rates: Fetches the latest currency conversion rates using an API.

User-friendly interface: Simple UI with dropdown selections for "From" and "To" currencies.

Automatic calculations: Converts currency values instantly based on selected rates.

Input validation: Ensures that only numeric values are entered for conversion.

Clear functionality: Users can reset inputs with a single click.

Error handling: Displays meaningful error messages for invalid inputs or API failures.

📌 Technologies Used
C#

WPF (Windows Presentation Foundation)

JSON.NET (Newtonsoft.Json)

HTTP Client for API calls

MVVM Architecture (partial implementation)

🚀 How It Works
On launch, the app fetches the latest currency exchange rates from the API.

Users enter an amount and select currencies for conversion.

Clicking the "Convert" button performs the conversion based on the latest exchange rates.

The app displays the converted currency value instantly.

Users can reset the fields using the "Clear" button.

📦 Prerequisites
.NET Framework (or .NET Core with WPF support)

API Key from Open Exchange Rates

Visual Studio (for running the project)

🛠 Setup & Installation
Clone the repository:

bash
Copy
Edit
git clone https://github.com/your-username/CurrencyConverter_WPF.git
Open the solution (.sln) file in Visual Studio.

Replace the API key in GetValue() method:

csharp
Copy
Edit
val = await GetData("https://openexchangerates.org/api/latest.json?app_id=YOUR_API_KEY");
Build and run the project.

