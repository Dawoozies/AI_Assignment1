using System.Collections.Generic;
using UnityEngine;
public class ObjectLookUp : MonoBehaviour
{
    public static ObjectLookUp ins;
    private void Awake()
    {
        ins = this;
    }
    public enum ObjectID
    {
        Town,
        Ammo,
        HealthPack,
        Blood,
        AmmoDepleted,
        HealthPackDepleted,
        BloodDepleted,
    }
    public Dictionary<ObjectID, List<WorldObject>> Objects = new();
    public Dictionary<object, float> sqrDistLookUpTable = new Dictionary<object, float>();
    public bool TryGetClosestObjectWithID(Vector3 point, ObjectID id, out WorldObject target)
    {
        target = null;
        if (!Objects.ContainsKey(id))
            return false;
        if (Objects[id].Count == 0)
            return false;

        sqrDistLookUpTable.Clear();
        Objects[id].Sort((WorldObject a, WorldObject b) =>
        {
            float sqrDstA = 0f;
            float sqrDstB = 0f;
            if (!sqrDistLookUpTable.ContainsKey(a))
            {
                sqrDstA = (a.transform.position - point).sqrMagnitude;
                sqrDistLookUpTable.Add(a, sqrDstA);
            }
            else
            {
                sqrDstA = sqrDistLookUpTable[a];
            }
            if (!sqrDistLookUpTable.ContainsKey(b))
            {
                sqrDstB = (b.transform.position - point).sqrMagnitude;
                sqrDistLookUpTable.Add(b, sqrDstB);
            }
            else
            {
                sqrDstB = sqrDistLookUpTable[b];
            }
            return sqrDstA.CompareTo(sqrDstB);
        });
        target = Objects[id][0];
        return true;
    }
    public void AddObject(WorldObject worldObject)
    {
        if (!Objects.ContainsKey(worldObject.id))
        {
            Objects.Add(worldObject.id, new List<WorldObject> { worldObject });
            return;
        }
        Objects[worldObject.id].Add(worldObject);
    }
    public void ChangeState(WorldObject worldObject, ObjectID newID)
    {
        if (!Objects.ContainsKey(newID))
        {
            Objects.Add(newID, new List<WorldObject> { worldObject });

            if (Objects[worldObject.id].Contains(worldObject))
                Objects[worldObject.id].Remove(worldObject);

            worldObject.id = newID;
            return;
        }

        if (Objects[worldObject.id].Contains(worldObject))
            Objects[worldObject.id].Remove(worldObject);

        Objects[newID].Add(worldObject);
        worldObject.id = newID;
    }
    //Entity look up
    public Dictionary<EntityType, List<Entity>> Entities = new();
    public bool TryGetClosestEntityOfType(Vector3 point, EntityType entityType, out Entity target)
    {
        target = null;
        if (!Entities.ContainsKey(entityType))
            return false;
        if (Entities[entityType].Count == 0)
            return false;

        sqrDistLookUpTable.Clear();
        Entities[entityType].Sort((Entity a, Entity b) =>
        {
            float sqrDstA = 0f;
            float sqrDstB = 0f;
            if (!sqrDistLookUpTable.ContainsKey(a))
            {
                sqrDstA = (a.transform.position - point).sqrMagnitude;
                sqrDistLookUpTable.Add(a, sqrDstA);
            }
            else
            {
                sqrDstA = sqrDistLookUpTable[a];
            }
            if (!sqrDistLookUpTable.ContainsKey(b))
            {
                sqrDstB = (b.transform.position - point).sqrMagnitude;
                sqrDistLookUpTable.Add(b, sqrDstB);
            }
            else
            {
                sqrDstB = sqrDistLookUpTable[b];
            }
            return sqrDstA.CompareTo(sqrDstB);
        });

        for (int i = 0; i < Entities[entityType].Count; i++)
        {
            if (Entities[entityType][i].state == State.Dead)
            {
                continue;
            }
            target = Entities[entityType][i];
            return true;
        }

        return false;
    }
    public void AddEntity(Entity entity)
    {
        if (!Entities.ContainsKey(entity.entityType))
        {
            Entities.Add(entity.entityType, new List<Entity> { entity });
            return;
        }
        Entities[entity.entityType].Add(entity);
    }
}
