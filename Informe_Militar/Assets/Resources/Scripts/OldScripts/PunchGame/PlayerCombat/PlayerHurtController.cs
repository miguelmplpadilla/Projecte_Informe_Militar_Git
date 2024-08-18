using System;
using System.Threading.Tasks;
using DG.Tweening;
using Resources.Scripts.PunchGame.PlayerCombat;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHurtController : MonoBehaviour
{
    public float maxLife = 15;
    [NonSerialized] public float life = 15;

    private PlayerCombatModel playerCombatModel;
    private PlayerModel model;
    public SpriteRenderer spriteRenderer;
    
    public Material normalMaterial;
    public Material blinkMaterial;
    
    public Image imageLifeBar;

    private void Awake()
    {
        life = maxLife;
        playerCombatModel = GetComponentInParent<PlayerCombatModel>();
        model = GetComponentInParent<PlayerModel>();
    }

    public async void Hurt()
    {
        if (playerCombatModel.dashing || !playerCombatModel.canHurt) return;

        playerCombatModel.canHurt = false;
        
        life--;
        
        imageLifeBar.DOFillAmount(life / maxLife, 0.2f);

        playerCombatModel.canMove = false;
        
        spriteRenderer.material = blinkMaterial;
        await Task.Delay(100);
        spriteRenderer.material = normalMaterial;

        if (life <= 0)
        {
            model.animator.SetTrigger("die");
            return;
        }
        
        playerCombatModel.canMove = true;
        
        await Task.Delay(1000);
        
        playerCombatModel.canHurt = true;
    }
}
