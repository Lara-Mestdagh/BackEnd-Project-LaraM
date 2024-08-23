# Core Features

## Character Sheet Management

### Basic Info

Store and manage essential character details like name, race, class, level, basic attributes (strength, dexterity, etc.).

### Health Tracking

Track current and maximum hit points, temporary hit points, and any conditions (e.g., poisoned, stunned).

### Movement Speeds

- **Walking Speed**: The default speed for most characters.
- **Optional Speeds**: Include fields for flying speed and swimming speed for characters with these abilities. These fields should be optional, as not all characters will have them.

### Vision Capabilities

Track the type and range of vision (e.g., darkvision range).

## Inventory Management

### Personal Inventory

Manage each character's inventory, including items, weapons, armor, and gold.

### Encumbrance Calculation

Automatically calculate whether a character is encumbered based on their carrying capacity and the total weight of items in their inventory.

### Shared Inventory

Items can be marked as "shared," making them accessible to the entire party. This could include shared funds or communal magical items.

## Party Management

### Party Overview

Provide a summary of the entire party’s health, conditions, movement speeds, and shared inventory. This is essential for quick reference during sessions.

## File Management

### Character Image Upload

Allow the DM to upload images for each character, which can be useful for visualization and session planning.

### Inventory Export

Provide functionality to download a character's inventory or the shared inventory as a PDF or CSV file. This helps in bookkeeping or sharing with players.

## Security and Access

### JWT Authentication

Secure the API using JWT tokens, ensuring only the DM or authorized users can access and modify character data.

## Versioning and Optimization

### API Versioning

Implement API versioning to allow for future expansions without breaking existing functionality.

### Caching

Implement caching for frequently accessed data, like character lists and inventories, to improve performance.

### Background Services

Utilize background tasks for any operations that might require more time, such as generating inventory reports.

## Blazor UI

### Character Overview Page

A simple interface where the DM can view and update character details, including movement speeds.

### Inventory Management Interface

An interface to manage both personal and shared inventories.

### Party Overview Interface

A summary page showing the status of the entire party.

## API Endpoints

### Character Endpoints

- **GET /api/characters** – Retrieve a list of all characters.
- **GET /api/characters/{id}** – Retrieve details of a specific character.
- **POST /api/characters** – Create a new character with basic info (name, race, class, etc.).
- **PUT /api/characters/{id}** – Update character details, including HP, conditions, vision capabilities, and movement speeds (walking, flying, swimming).
- **DELETE /api/characters/{id}** – Soft delete a character (mark as inactive).

### Inventory Endpoints

- **GET /api/characters/{id}/inventory** – Retrieve the inventory for a specific character.
- **POST /api/characters/{id}/inventory** – Add an item to a character's inventory.
- **PUT /api/characters/{id}/inventory/{itemId}** – Update a specific item in the inventory (e.g., quantity, weight, shared status).
- **DELETE /api/characters/{id}/inventory/{itemId}** – Soft delete an item from the inventory (mark as removed but keep it for record).
- **GET /api/party/inventory** – Retrieve the shared inventory for the entire party.
- **GET /api/party/overview** – Retrieve a summary of the party’s health, conditions, movement speeds, and shared inventory.

### File Management Endpoints

- **POST /api/files/upload** – Upload a character image.
- **GET /api/files/download/inventory/{characterId}** – Download a character’s inventory as a PDF or CSV file.
- **GET /api/files/download/party-inventory** – Download the shared inventory as a PDF or CSV file.
