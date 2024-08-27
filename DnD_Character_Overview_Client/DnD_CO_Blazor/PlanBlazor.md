# Blazor

- 1. Overview Page
Purpose:

Provide a comprehensive overview of both character and inventory information for the entire party. This page serves as a central hub for the DM to quickly assess the status of the game.
Features:

Character Information Summary: Display a summary of all characters (both Player Characters and DM Characters), including key details such as name, race, class, current health, conditions, and movement speeds.
Inventory Overview: Include a summary of the shared inventory, such as communal gold, shared magical items, and other party-wide resources.
Quick Navigation: Buttons or links to quickly navigate to detailed character pages or the inventory management page.
Layout Suggestions:

Graphical Representation: Use a combination of cards or tiles for each character, showing essential information at a glance. Differentiate Player Characters from DM Characters using distinct colors or icons.
Shared Inventory Panel: A dedicated panel or section on the same page summarizing the shared inventory. This panel could also show quick stats (e.g., total weight, encumbrance status, total gold).
Quick Actions: Provide quick action buttons (e.g., "Heal All," "Rest," "View Detailed Inventory") to enable swift in-game management.

- 2. All Characters Page
Purpose:

Manage all characters in the game, providing functionality to add, update, or delete characters. This page will differentiate between Player Characters and DM Characters and allow the DM to perform both soft and hard deletes.
Features:

Character List: A detailed list or grid displaying all characters, with a visual distinction between Player Characters and DM Characters.
Character Management: Options to add new characters, edit existing ones, or delete characters (soft delete with the option for a hard delete for permanent removal).
Filter and Sort: Filtering options to view only Player Characters, only DM Characters, or all characters. Sorting capabilities to organize characters by name, race, class, or other attributes.
Layout Suggestions:

Character Cards or Table View: Use a card layout or table view to display detailed information for each character. Include buttons for actions like "Edit," "Delete," and "View Inventory" directly on each card or row.
Toggle for Player/DM Characters: A toggle or filter to switch between viewing Player Characters and DM Characters.
Add/Edit Character Modal: Use modals for adding or editing characters to keep the user on the same page and maintain context.
Confirmation Dialogs: Implement confirmation dialogs for delete actions, especially for hard deletes, to prevent accidental data loss.

- 3. Inventory Page
Purpose:

Provide a detailed view of both individual character inventories and the shared party inventory. Enable management of items, including adding, updating, deleting, and moving items between personal and shared inventories.
Features:

Shared Inventory Overview: Display a list of items in the shared inventory, including quantities, weight, and shared status.
Character Inventory List: Show a clickable list of all characters, where clicking on a character opens up their detailed inventory view.
Inventory Management: Options to add new items to either the shared or personal inventories, update existing items (e.g., change quantity, weight, or description), and delete items.
Move Items Between Inventories: Functionality to move items from a personal inventory to the shared inventory and vice versa.
Download Inventory: Provide options to download the inventory in CSV or PDF format for both shared and individual inventories.
