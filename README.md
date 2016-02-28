# ProjectCrusade

##First project of RHS Software Development Club

A tile-based game, the storyline of which we have not yet finalized. 

###Tiled
For level design, it is best to use the Tiled level editor. You can download Tiled at:

<a href="https://thorbjorn.itch.io/tiled/download/eyJleHBpcmVzIjoxNDU2Njk2NDA2LCJpZCI6Mjg3Njh9.kgiiCDqF%2frsfsS74AvvKNNH71W8%3d">https://thorbjorn.itch.io/tiled/download/eyJleHBpcmVzIjoxNDU2Njk2NDA2LCJpZCI6Mjg3Njh9.kgiiCDqF%2frsfsS74AvvKNNH71W8%3d</a>

After opening Tiled, you must import the main world file (Content/Levels/world.tmx). After each change, save the world file and EXPORT it using Cmd+E. 

When editing the world, be sure to distinguish between the Floor and Wall layers. The player collides with all tiles on the Wall layer. 

This creates two files: one for walls, and one for floors. 
