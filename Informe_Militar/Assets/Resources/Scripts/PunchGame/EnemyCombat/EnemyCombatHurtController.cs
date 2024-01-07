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

    public Material normalMaterial;
    public Material blinkMaterial;

    public int combo = 0;

    public float knockBackForce = 2;

    private void Awake()
    {
        model = GetComponentInParent<EnemyCombatModel>();
    }

    public async void Hurt()
    {
        if (!model.canHit || !model.canMove) return;

        model.life--;

        imageLifeBar.DOFillAmount(model.life / model.maxLife, 0.2f);

        if (model.life <= 0)
        {
            model.animator.SetTrigger("die");
            model.canMove = false;

            spriteRenderer.material = blinkMaterial;
            await Task.Delay(100);
            spriteRenderer.material = normalMaterial;

            return;
        }

        model.canMove = false;

        StopCoroutine("RestartCombo");
        combo++;

        if (combo == 3)
            KnockBack();

        StartCoroutine("RestartCombo");

        spriteRenderer.material = blinkMaterial;
        await Task.Delay(100);
        spriteRenderer.material = normalMaterial;

        await Task.Delay(100);

        model.canMove = true;
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
    }

    private IEnumerator RestartCombo()
    {
        yield return new WaitForSeconds(3);

        combo = 0;
    }
}
