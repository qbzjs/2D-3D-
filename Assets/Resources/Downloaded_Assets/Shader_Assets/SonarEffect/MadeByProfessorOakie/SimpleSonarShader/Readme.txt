Hello! Thanks for buying this shader. I've put time into making it and know it pretty well so please reach out to me on twitter @ProfessorOkie if you have any questions, comments, suggestions, issues, or make something cool using this and want to share :)

I've included a whole series of examples that I called the Color Minis. They are a set of basic uses of the sonar shader and you can pull them apart and copy things from them. 

---- 
Setup: 
1. Open Edit->Project Settings->Graphics
2. Change Scriptable Render Pipeline Settings to ColorMinis_URPAsset
Now all the examples should work!

If you already have your own Pipeline Asset then you can keep that but just copy the Renderer Feature from one of the sonar ones onto your asset. 

----
Integrate into your scene: 
You have to decide how you want the rings to be spawned. If you already have a script with the spot then just call this function: SimpleSonarShader_SonarSender.Instance.StartSonarRing(position, intensity). 
If you want them to happen on all collisions then look at my example scenes for my helper setup using the AddComponentToColliders script to add SimpleSonarShader_SendOnCollision to all the geometry in the scene. 
If you want them to come from specific points like an enemy or an objective then look at my Duality example where I use SimpleSonarShader_ExamplePulse.

----
How to customize: 
1. Create a material and set the material to MadeByProfessorOakie/SimpleSonarShader
2. Play around with all the values to get what you like. 
3. You can have different types of rings by using the different indices like my example in Duality. 
