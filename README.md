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
