# Pokémon Search App

A minimal full-stack demo that lets you look up Pokémon by name or ID.  
Backend is a .NET 8 minimal API (SQLite + EF Core + JWT), frontend is Vue 3 + Vite (Pinia + Tailwind).

---

![image](https://github.com/user-attachments/assets/1c6b554e-446b-4609-b988-705cabe062ba)

---

## Features

- Search any Pokémon by name or ID  
- Displays sprite, types, abilities, height & weight  
- Results cached in a local SQLite database  
- JWT-protected API endpoints with simple login  
- Responsive, mobile-friendly UI  
- Custom Pokémon logo font and styling  

---

## Tech Stack

- **Backend**: .NET 8 minimal API, C#  
- **Database**: SQLite via EF Core  
- **Auth**: JWT (HMAC-SHA256)  
- **Frontend**: Vue 3, TypeScript, Vite  
- **State**: Pinia  
- **Styling**: Tailwind CSS  
- **API client**: Axios  
- **Docs**: Swagger / OpenAPI  

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- [Node 20+ & npm](https://nodejs.org/)  
- Git  

---

## Setup & Run

1. **Clone the repo**
   
   ```bash
   git clone https://github.com/Dalej122/pokemon-information-app
   cd pokemon-information-app

3. **Run the API in one terminal**
   ```bash
   cd PokemonApi
   dotnet restore
   dotnet run
- The API listens on http://localhost:5100
- Swagger UI: http://localhost:5100/swagger

3. **Run the frontend in another terminal**
   
   ```bash
   cd pokemon-web
   npm install
   npm run dev
- The app runs on http://localhost:5173 (or the port shown by Vite)

4. **Use the app**
- Browse to http://localhost:5173
- You’ll be redirected to /login
- Sign in with:
  
  ```makefile
  Username: admin
  Password: Pa$$w0rd!
- Enter a Pokémon name (e.g. “pikachu”) or ID (e.g. “25”) and click Search

## Notes

- A pre-built pokemon.db is included in the repo so you don’t need to run EF migrations manually.
- To reset the cache, delete PokemonApi/pokemon.db and restart the API.
