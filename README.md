# ProjectCrusade

##First project of RHS Software Development Club

A tile-based game, the storyline of which we have not yet finalized. 

[Please visit the wikis for information on style, story, and gameplay.](https://github.com/BoolClub/ProjectCrusade/wiki/ProjectCrusade-Wiki)

###Tiled
For level design, it is best to use the Tiled level editor. You can download Tiled at:

<a href="https://thorbjorn.itch.io/tiled/download/eyJleHBpcmVzIjoxNDU2Njk2NDA2LCJpZCI6Mjg3Njh9.kgiiCDqF%2frsfsS74AvvKNNH71W8%3d">https://thorbjorn.itch.io/tiled/download/eyJleHBpcmVzIjoxNDU2Njk2NDA2LCJpZCI6Mjg3Njh9.kgiiCDqF%2frsfsS74AvvKNNH71W8%3d</a>

After opening Tiled, you must import the main world file (Content/Levels/world.tmx). After each change, save the world file and EXPORT it using Cmd+E. 

When editing the world, be sure to distinguish between the Floor and Wall layers. The player collides with all tiles on the Wall layer. 

This creates two files: one for walls, and one for floors. 


##Tutorials
Several of these tutorials assume you have a room's .TMX file opened in the Tiled map editor.
###Adding lights to rooms
Assuming you have a room .TMX file open in Tiled, create a new object layer titled `Lights` (case sensitive). Then, draw a rectangle at the desired location. The upper-left-hand corner of this rectangle becomes the location of the point light. To change the properties of the light, add the following properties using the plus button in the properties panel:

1. `brightness` - light brightness value
2. `red` - red value between 0 and 1
3. `green` - green value between 0 and 1
4. `blue` - blue value between 0 and 1

###Adding entities to rooms
Create a new object layer titled `Entities` (again, case sensitive). Then, draw a rectangle at the desired location for the new NPC/entity. The upper-left-hand corner of this rectangle becomes the initial position of the entity. To change the properties of the entity, add the following properties using the plus button in the properties panel:

1. `name` - a required string property. Used by the game to determine which type of entity to place. So far, there is one possible value for this property (more to come): 
    - `npc` - a friendly NPC with whom the player can interact
    - `chest` - a repository for an item
2. `message` - a string property for use with `name = npc` identifier. Used in the NPC's text box. To delineate multiple messages, use the '\' character. Example:
    - `This is a message.\This is another message.\This is yet another message.`
3. `type` - for use with `name = chest` identifier; indicates what item the chest will contain. This corresponds directly with the item's class name. For instance, 
    - `Apple`
    - `StarterArrow`
    - etc.
4. `count` - optional, for use with `name = chest` identifier; indicates how many items the chest will contain. When adjusting this property, ensure that the item is stackable. 


###Adding/changing fonts (sorry if a bit complicated)

In MonoGame (XNA) it is necessary to include a .spritefont and .xnb file for each font. In the Content/Fonts folder, you can find all 
fonts in the game. 

To add a new font, just make a copy of any existing .spritefont file in this directory. When you're finished editing it
(i.e. changing font family and size), you must compile the font into an .xnb file. First, you must edit the .mgcb file in the Pipeline tool
so that the .spritefont file is part of the project. Next, you must compile the .xnb files: this can be done by running the following in Terminal 
command (ensuring the project is in the right place):

cd ~/Projects/ProjectCrusade/ProjectCrusade/Content/Fonts

mono /Applications/Pipeline.app/Contents/MonoBundle/MGCB.exe /@:~/Projects/ProjectCrusade/ProjectCrusade/Content/Content.mgcb


Drag the .xnb file from the generated bin directory into the same directory as the .spritefont (Content/Fonts)
