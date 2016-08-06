using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrusade
{
	//<summary> This class represents the GUI that displays the player's sanity. </summary>
	public class HealthBarGUI
	{
		Vector2 position;
		float health;
		float maxHealth;
		const int Height = 32;
		const int Width = 256;
		const float animRate = 0.1f;
	

		Color barColor = new Color ();

		public HealthBarGUI (float healthAmount) {
			health = healthAmount;
			position = new Vector2 (MainGame.WindowWidth / 2 - Width/2, MainGame.WindowHeight - Height);
		}

		public void Update(GameTime time, float healthAmount, float maxHealth) {
			health += (healthAmount-health)*animRate;
			this.maxHealth = maxHealth;
			float fracHealth = healthAmount / maxHealth;
			float interpRed = MathHelper.Clamp (1 - 2 * fracHealth, 0, 1);
			float interpYellow = 2 * (fracHealth < 0.5f ? (fracHealth) : (1 - fracHealth));
			float interpGreen = MathHelper.Clamp (2 * fracHealth - 1, 0, 1);
			barColor = new Color (Color.Red.ToVector3 () * interpRed + Color.Yellow.ToVector3 () * interpYellow + new Vector3(0,1.0f,0) * interpGreen);
//
//			if (sanity <= maxHealth && sanity > 60) {
//				barColor = new Color(0,255,0);
//			} else if (sanity <= 60 && sanity > 30) {
//				barColor = Color.Yellow;
//			} else {
//				barColor = Color.Red;
//			}
		}

		public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, FontManager fontManager) {
			spriteBatch.Draw (textureManager.GetTexture("healthBarFill"), position,null,new Rectangle(0,0,(int)(health/maxHealth*Width), Height), null, 0, null, barColor, SpriteEffects.None, 0.0f);
			spriteBatch.Draw (textureManager.GetTexture("healthBar"), position,null,null, null, 0, null, Color.White, SpriteEffects.None, 0.0f);
		}

	} //END OF SANITYBARGUI CLASS



} //END OF NAMESPACE

