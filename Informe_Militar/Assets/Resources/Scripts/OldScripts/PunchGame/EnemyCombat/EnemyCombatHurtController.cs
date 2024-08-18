using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombatHurtController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Image imageLifeBar;

    private EnemyCombatModel model;
    private EnemyCombatController enemyCombatController;

    public Material normalMaterial;
    public Material blinkMaterial;

    public int combo = 0;

    public float knockBackForce = 2;

    private void Awake()
    {
        model = GetComponentInParent<EnemyCombatModel>();
        enemyCombatController = GetComponentInParent<EnemyCombatController>();
    }

    public async void Hurt()
    {
        if (!model.canHit || !model.canMove) return;

        int numRan = Random.Range(0, 5);

        if (numRan == 0)
        {
            Dash();
            return;
        }

        model.life--;

        //model.animator.SetTrigger("idle");
        model.atack = false;
        model.atacking = false;
        model.currentTimeAtack = 0;

        imageLifeBar.DOFillAmount(model.life / model.maxLife, 0.2f);

        if (model.life <= 0)
        {
            Die();
            return;
        }

        model.canMove = false;

        StopCoroutine("RestartCombo");
        combo++;

        bool knockedBack = false;

        if (combo == 3)
        {
            knockedBack = true;
            KnockBack();
        }

        StartCoroutine("RestartCombo");

        spriteRenderer.material = blinkMaterial;
        await Task.Delay(100);
        spriteRenderer.material = normalMaterial;

        await Task.Delay(100);

        if (Random.Range(0, 2) == 0 && !knockedBack)
            Dash();
        
        model.canMove = true;
    }

    private async void Die()
    {
        model.animator.SetTrigger("die");
        model.canMove = false;

        model.rb.AddForce(
        new Vector2(transform.position.x < model.player.transform.position.x ? -1 : 1, 0) * knockBackForce,
        ForceMode2D.Impulse);

        spriteRenderer.material = blinkMaterial;
        await Task.Delay(100);
        spriteRenderer.material = normalMaterial;

        await Task.Delay(100);

        model.rb.velocity = Vector2.zero;
    }

    private async void Dash()
    {
        model.dashing = true;

        model.rb.velocity = Vector2.zero;

        model.animator.SetTrigger("dash");

        Vector2 direction = new Vector2(transform.localScale.x > 0 ? 1 : -1, 0);

        model.rb.AddForce(direction * 5, ForceMode2D.Impulse);

        await Task.Delay(250);

        model.rb.velocity = Vector2.zero;
        model.dashing = false;
    }

    private async void KnockBack()
    {
        model.canHit = false;

        model.animator.SetTrigger("knock");
        combo = 0;

        model.rb.velocity = Vector2.zero;

        model.rb.AddForce(
            new Vector2(transform.position.x < model.player.transform.position.x ? -1 : 1, 0) * knockBackForce, 
            ForceMode2D.Impulse);

        await Task.Delay(200);

        model.rb.velocity = Vector2.zero;

        await Task.Delay(1800);

        model.animator.SetTrigger("getUp");

        await Task.Delay(300);

        model.canHit = true;

        int randomAction = Random.Range(0, 2);
        
        if (randomAction == 0) 
            enemyCombatController.Attack();
        else
           Dash();
    }

    private IEnumerator RestartCombo()
    {
        yield return new WaitForSeconds(3);

        combo = 0;
    }
}
