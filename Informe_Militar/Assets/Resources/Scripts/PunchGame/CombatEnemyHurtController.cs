using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class CombatEnemyHurtController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Material normalMaterial;
    public Material blinkMaterial;

    public int combo = 0;

    public Animator animator;

    public bool canHit = true;

    public async void Hurt()
    {
        if (!canHit) return;

        StopCoroutine("RestartCombo");
        combo++;

        if (combo == 3)
            KnockBack();

        StartCoroutine("RestartCombo");

        spriteRenderer.material = blinkMaterial;
        await Task.Delay(100);
        spriteRenderer.material = normalMaterial;
    }

    private async void KnockBack()
    {
        canHit = false;

        animator.SetTrigger("knock");
        combo = 0;

        await Task.Delay(2000);

        animator.SetTrigger("getUp");

        await Task.Delay(300);

        canHit = true;
    }

    private IEnumerator RestartCombo()
    {
        yield return new WaitForSeconds(2);

        combo = 0;
    }
}
