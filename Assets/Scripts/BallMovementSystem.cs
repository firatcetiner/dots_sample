using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class BallMovementSystem : SystemBase
{
   [BurstCompile]
   protected override void OnUpdate()
   {
      var deltaTime = Time.DeltaTime;

      /*
       * Query the entities that has both Translation, Rotation and BallComponent IComponentData.
       * Then move the objects forward (e.g Vector3.forward in MonoBehavior approach)
       */
      Entities
         .ForEach((ref Translation translation, in Rotation rotation, in BallComponent ball) =>
         {
            translation.Value += ball.MoveSpeed * deltaTime * math.forward(rotation.Value);
         }).Schedule();
   }
}