using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WallTransparent : MonoBehaviour
{
    [SerializeField] private GameObject centerPlayer;

    [SerializeField] private List<GameObject> allWallsWithFade = new List<GameObject>();

    public Material wallOpaque;
    public Material wallTrasparent;

    private void Update()
    {
        CheckWalls();
    }

    private void CheckWalls()
    {
        RaycastHit[] allHits = Physics.RaycastAll(transform.position, centerPlayer.transform.position - transform.position);
        
        foreach (var hit in allHits)
        {
            if (hit.collider.CompareTag("TransparentWall") && !allWallsWithFade.Contains(hit.collider.gameObject))
            {
                float distancePlayerToCamera = Vector3.Distance(centerPlayer.transform.position, transform.position);
                float distanceWallToCamera = Vector3.Distance(hit.point, transform.position);
                if (distanceWallToCamera > distancePlayerToCamera) continue;
                
                FadeWall(hit.collider.GetComponent<Renderer>(), 0);
                allWallsWithFade.Add(hit.collider.gameObject);
            }
        }

        List<GameObject> currentWalls = new List<GameObject>(allWallsWithFade);
        
        foreach (var hit in allHits)
            foreach (var wall in allWallsWithFade)
                if (hit.collider.gameObject.Equals(wall)) currentWalls.Remove(wall);

        foreach (var wall in currentWalls)
        {
            FadeWall(wall.GetComponent<Renderer>(), 1);
            allWallsWithFade.Remove(wall);
        }
    }

    private void FadeWall(Renderer rendererWall, float finalAlpha)
    {
        rendererWall.enabled = true;
        
        if (rendererWall.material.color.a > 0) 
            rendererWall.material = wallTrasparent;
        
        rendererWall.material.DOKill();
        rendererWall.material.DOFade(finalAlpha, 0.3f).OnComplete(() =>
        {
            if (finalAlpha > 0)
            {
                rendererWall.material = wallOpaque;
                return;
            }
            
            rendererWall.enabled = false;
        });
    }
}
