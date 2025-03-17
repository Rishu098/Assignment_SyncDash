About Sync Dash

1. Automatically move the player forward and increase speed over time.  
2. Let the player move(space, left right) or (swipe up, left and right) to jump, left and right movement of the player
3. Spawn obstacles and orbs randomly using object pooling for better performance.  
4. Detect collisions with obstacles to end the game and with orbs to increase the score.  
5. Display the score and update it in real-time.  
6. Create a Network player on the left side to mirror the player’s actions.  
7. Use a queue to store player actions (e.g., jumps, movements) for syncing.  
8. Add a slight delay to the ghost player’s actions to simulate network lag.  
9. Smooth the Network player’s movements using interpolation to avoid jitter.  
10. Build a main menu with "Start" and "Exit" buttons.  
11. Show a game over screen with "Restart" and "Main Menu" options.    
12. Use object pooling to reuse obstacles and orbs instead of creating/destroying them.  
13. Optimize rendering with simple shaders and particle effects.  
14. Sync player and ghost player actions like jumps, movements, and orb collection.    
15. Add a score system based on distance and orbs collected.  
16. Use Unity’s UI tools to create menus and display the score.