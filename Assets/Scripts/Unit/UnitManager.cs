using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager instance;
        
        private List<Unit> unitList;
        private List<Unit> playerList;
        private List<Unit> enemyList;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            unitList = new List<Unit>();
            playerList = new List<Unit>();
            enemyList = new List<Unit>();
        }

        private void Start()
        {
            Unit.ON_ANY_UNIT_SPAWNED += Unit_OnAnyUnitSpawned;
            Unit.ON_ANY_UNIT_DEAD += Unit_OnAnyUnitDead;
        }

        private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
        {
            Unit unit = sender as Unit;
            
            unitList.Add(unit);
            
            if (unit.IsEnemy())
            {
                enemyList.Add(unit);
            }
            else
            {
                playerList.Add(unit);
            }
        }

        private void Unit_OnAnyUnitDead(object sender, EventArgs e)
        {
            Unit unit = sender as Unit;
            
            unitList.Remove(unit);
            
            if (unit.IsEnemy())
            {
                enemyList.Remove(unit);
            }
            else
            {
                playerList.Remove(unit);
            }
        }

        public List<Unit> GetUnits()
        {
            return unitList;
        }
        
        public List<Unit> GetEnemies()
        {
            return enemyList;
        }
        
        public List<Unit> GetPlayers()
        {
            return playerList;
        }
    }
}