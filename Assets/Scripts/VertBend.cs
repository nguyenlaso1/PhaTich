// @sonhg: class: VertBend
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VertBend : BaseMeshEffect//BaseVertexEffect
{
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        int vertCount = vh.currentVertCount;

        var vert = new UIVertex();
        for (int v = 0; v < vertCount; v++)
        {
            vh.PopulateUIVertex(ref vert, v);

            vert.position.z = (UnityEngine.Random.value - 0.5f) * 20f;

            vh.SetUIVertex(vert, v);
        }
    }

    public void Update()
    {
        var graphic = GetComponent<Graphic>();
        graphic.SetVerticesDirty();
    }
    //public override void ModifyVertices(List<UIVertex> verts)
    //{
    //	if (!this.IsActive())
    //	{
    //		return;
    //	}
    //	for (int i = 0; i < verts.Count; i++)
    //	{
    //		UIVertex value = verts[i];
    //		value.position.z = Mathf.Sin(value.position.x * this.scale + this.phase) * this.amplitude;
    //		verts[i] = value;
    //	}
    //}

    //public float scale = 10f;

    //public float amplitude = 35f;

    //public float phase;
}
