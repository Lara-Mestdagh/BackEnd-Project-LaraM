# Core Features

## Character Sheet Management

### Basic Info

Store and manage essential character details like name, race, and basic attributes (strength, dexterity, etc.).
Both PlayerCharacter and DMCharacter classes will inherit from a common CharacterBase class, which will contain shared properties.

### Health Tracking

Track current and maximum hit points, temporary hit points, and any conditions (e.g., poisoned, stunned).
PlayerCharacter and DMCharacter classes will utilize inherited properties for HP tracking.

### Movement Speeds

-**Walking Speed**: The default speed for most characters.
-**Optional Speeds**: Include fields for flying speed and swimming speed for characters with these abilities. These fields are optional, as not all characters will have them.
All movement speeds will be managed via the CharacterBase class.

### Vision Capabilities

Track the type and range of vision (e.g., darkvision range).
Vision-related properties will be part of the CharacterBase class.

## Inventory Management

### Personal Inventory

Manage each character's inventory, including items, weapons, armor, and gold.
Both PlayerCharacter and DMCharacter classes will have an InventoryItems collection inherited from CharacterBase.

### Encumbrance Calculation

Automatically calculate whether a character is encumbered based on their carrying capacity and the total weight of items in their inventory.
Encumbrance logic will be implemented in the service layer, leveraging the inherited inventory properties.

### Shared Inventory

Items can be marked as "shared," making them accessible to the entire party. This could include shared funds or communal magical items.
A SharedInventory model will manage shared items, accessible by all characters.

## Party Management

### Party Overview

Provide a summary of the entire party’s health, conditions, movement speeds, and shared inventory. This is essential for quick reference during sessions.
The party overview will aggregate data from all PlayerCharacter and DMCharacter instances.

## File Management

### Character Image Upload

Allow the DM to upload images for each character, useful for visualization and session planning.
PlayerCharacter and DMCharacter classes will have an ImagePath property inherited from CharacterBase to store the path to the uploaded image.

### Inventory Export

Provide functionality to download a character's inventory or the shared inventory as a PDF or CSV file. This helps in bookkeeping or sharing with players.
Implement endpoints for inventory export in both formats (CSV and PDF).

### Character Sheet Export

Allow exporting a character's sheet in CSV or PDF format for easy sharing or printing.
Implement endpoints for character sheet export in both formats (CSV and PDF).

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

- **GET /api/player-characters** – Retrieve a list of all player characters.
- **GET /api/player-characters/{id}** – Retrieve details of a specific player character.
- **POST /api/player-characters** – Create a new player character with basic info (name, race, class, etc.).
- **PUT /api/player-characters/{id}** – Update player character details, including HP, conditions, vision capabilities, and movement speeds (walking, flying, swimming).
- **DELETE /api/player-characters/{id}** – Soft delete a player character (mark as inactive).

- **GET /api/dm-characters** – Retrieve a list of all DM characters.
- **GET /api/dm-characters/{id}** – Retrieve details of a specific DM character. (gRPC)
- **POST /api/dm-characters** – Create a new DM character with basic info (name, race, class, etc.).
- **PUT /api/dm-characters/{id}** – Update DM character details, including HP, conditions, vision capabilities, and movement speeds (walking, flying, swimming).
- **DELETE /api/dm-characters/{id}** – Soft delete a DM character (mark as inactive).

### Inventory Endpoints

- **GET /api/characters/{id}/inventory** – Retrieve the inventory for a specific character. (gRPC)
- **POST /api/characters/{id}/inventory** – Add an item to a character's inventory. (gRPC)
- **PUT /api/characters/{id}/inventory/{itemId}** – Update a specific item in the inventory (e.g., quantity, weight, shared status). (gRPC)
- **DELETE /api/characters/{id}/inventory/{itemId}** – Soft delete an item from the inventory (mark as removed but keep it for record).
- **GET /api/party/inventory** – Retrieve the shared inventory for the entire party. (gRPC)
- **GET /api/party/overview** – Retrieve a summary of the party’s health, conditions, movement speeds, and shared inventory. (gRPC)

### File Management Endpoints

- **POST /api/files/upload** – Upload a character image.
- **GET /api/files/download/inventory/{characterId}/{format}** – Download a character’s inventory in the specified format (CSV or PDF).
- **GET /api/files/download/party-inventory/{format}** – Download the shared inventory in the specified format (CSV or PDF).
- **GET /api/files/download/character-sheet/{characterId}/{format}** – Download a character’s sheet in the specified format (CSV or PDF).
