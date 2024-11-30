
# Exchange Service API

This API provides exchange rate services to convert currencies.

## Features

- Fetch exchange rates for various currencies.
- Convert amounts from one currency to another.
- Supports JSON payloads for API interaction.

## Usage

### Endpoint: Convert Currency

- **URL**: `http://localhost:5088/ExchangeService`
- **Method**: `POST`
- **Headers**:
  - `accept: text/plain`
  - `Content-Type: application/json`
- **Request Body**:
  ```json
  {
    "amount": 5,
    "inputCurrency": "AUD",
    "outputCurrency": "USD"
  }

## ⚠️ Important

**Please Note:** The API must be running locally at `http://localhost:5088` before you execute the `curl` command above.

## How to Run the application

- 1.Make sure the Startup project is Kindred.CurrencyExchangeService
- 2.Run the Project
- 3.Run the Curl script to get the Output.

