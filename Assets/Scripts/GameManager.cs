using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace
{
   public class GameManager : MonoBehaviour
   {
      private EntityManager _entityManager;


      /*
       * Assign Mesh and Material from the editor.
       */
      [SerializeField] private Mesh ballMesh;
      [SerializeField] private Material ballMaterial;

      [SerializeField] private bool useEcs;

      private readonly List<GameObject> _list = new List<GameObject>();

      private void Start()
      {
         // Get the default EntityManager from the default world.
         _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
         if (useEcs)
         {
            SpawnWithEntities();
         }
         /*
          * Create old fashion primitive GameObjects. (10.000).
          */
         else
         {
            for (var i = 0; i < 10000; i++)
            {
               var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
               ball.GetComponent<Renderer>().material = ballMaterial;
               _list.Add(ball);
            }
         }
      }


      private void Update()
      {
         /*
          * We need to use Update when dealing with GameObjects.
          */
         if (!useEcs)
         {
            _list.ForEach(o => { o.transform.position = Vector3.forward * Time.deltaTime; });
         }
      }

      /*
       * Creates 10.000 entities in the default entity world.
       * BallComponent is added since we are going to use its data later on OnUpdate() in BallMovementSystem.
       */
      private void SpawnWithEntities()
      {
         
         /*
          * Using same EntityArchetype for all entities.
          */
         var archetype = _entityManager.CreateArchetype(
            typeof(Translation), typeof(Rotation), typeof(RenderMesh), typeof(RenderBounds), typeof(LocalToWorld));

         /*
          * Create entities using default EntityManager. This can be optimized later.
          */
         var entities = _entityManager.CreateEntity(archetype, 10000, Allocator.Temp);
         foreach (var entity in entities)
         {
            _entityManager.AddComponentData(entity, new Translation
            {
               Value = new float3(0, 0, 0)
            });

            _entityManager.AddComponentData(entity, new Rotation
            {
               Value = quaternion.identity // use new mathematics library instead of Quaternion.Identity
            });

            _entityManager.AddSharedComponentData(entity, new RenderMesh
            {
               mesh = ballMesh,
               material = ballMaterial
            });

            _entityManager.AddComponentData(entity, new BallComponent
            {
               MoveSpeed = 5.0f,
               Tag = 1
            });
         }

         /*
          * Since entities are created we don't need the array storing their references.
          * Releasing the resources.
          */
         entities.Dispose();
      }
   }
}