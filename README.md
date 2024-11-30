
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

## Two different approach

- Approach 1.
  - Have the ApiUrL as "https://open.er-api.com/v6/latest/"
  - Appen the ApiUrl with the InputCurrency
  - Example.. If the InputCurrency is AUS,
    - Append the InputCurrency to the Url   "https://open.er-api.com/v6/latest/AUS"
    - Retrieve the Exchange rate and multiply the requested amount with the rate for the OutputCurrency to get the result.
- Approach 2.(Implemented)
  - Have the ApiUrL as "https://open.er-api.com/v6/latest/USD"
  - Always get the currency exchanges rates for USD base currency and derive the rate for the InputCurrency relative to the USD.
  - Formula: (Amount/InputCurrencyRate)* OutputCurrencyRate.
  - Should round to 2 digits. ** Will have minor difference in decimals to Approach 1 becuase of rounding **

## Enhancement.    
According to the Response, It gives the next update time. If it me ans that the rate wont change till the next update time, The CurrencyExchange rates can be cached in Redis and can be refreshed. 

## Test Coverage.
Test coverage is Added but with TODO for few Items..

