# 3 Space Defence 2
For this assignment we are going to continue with the space defence game, adding the finishing touches to turn Space Defence into a real game. Make sure to hand in Space Defence 1 before continuing with the new assignment.

To expand the game we increase the playing area beyond the screen, we add more enemies, another weapon and a new mechanic, where the player can score by shipping goods between 2 planets.

All the sprites you need can be found in the assets folder, but you are also free to use your own!

### Functionality:
Implement the following functionality:

#### Add a start and pause screen:
- [ ] Add a start screen to the game from where the player can at least start or quit the game. (0.5p)
- [ ] Add a pause screen from where the player can at least continue or quit the game. The game should still be visible in the background, but not be updated.  (0.5p)

#### Expand the level:
- [x] Create a camera class that follows the player as it moves around the level and expand the play area. (1 p)

#### More enemies:
- [ ] Add a new asteroid enemy that does not move, and destroys both the player and enemies on touch. (0.5p)
- [ ] Spawn more enemies over time, slowly ramping up the difficulty. (0.5p)

#### Animations:
- [ ] Implement a class that can play an animation using a sprite sheet. (0.5p)
- [ ] It should at least be possible to adjust the speed of the animation and whether or not it loops in the class.
- [ ] Whenever a player or an alien dies, play an explosion animation (0.5p)

#### Game Goal (HUD):
- [ ] Add at least 2 planets to the level. When the player goes to one planet, they pick up cargo. At the other planet they can drop it off. (0.5p)
- [ ] Whenever the player drops off cargo they score points. The score should always be visible on screen. (HUD) (1p)
- [ ] There should always be some indication of whether or not the player is carrying any cargo. (HUD) (1p)

#### New Weapon:
- [ ] Extract a weapon class for the two current weapons that can be added to the ship as a component to the ship. (1p)
- [ ] Add a third weapon, the player can pick up. (0.5)
- [ ] Add some unique functionality to the weapon. You can decide what it does. It can for example be an explosion, or a lightning weapon that also hits nearby targets (1p)

#### Expand the Game:
Add something new to the game of your choosing. For example: 
- [ ] More enemy types (.5-2p depending on complexity of the enemies)
- [ ] Health bars (~1p for health bars that follow both enemies and the player)
- [ ] Weapon upgrades (1-2p depending on depth)

Add a short description of what you added. You can get up to 2 points depending on complexity 

You can get a maximum of 11 points for this assignment.

The final grade for Space defence is (your points for the first part + the second part) /2 with a maximum of 10
