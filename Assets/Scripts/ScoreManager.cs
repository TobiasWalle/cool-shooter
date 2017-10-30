using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Assets.Core;

public class ScoreManager : NetworkBehaviour
{
    private readonly PlayerSyncList _players = new PlayerSyncList();
    private int _changeCounter;

    private void Start()
    {
        _changeCounter = 0;
    }

    [ClientRpc]
    public void RpcIncrementDeaths(string playerId)
    {
        this.IncrementScore(playerId, ScoreType.Deaths);
    }

    [ClientRpc]
    public void RpcIncrementKills(string playerId)
    {
        this.IncrementScore(playerId, ScoreType.Kills);
    }

    private void IncrementScore(string playerId, ScoreType type)
    {
        var i = _players.FindIndexById(playerId);
        if (i < 0)
        {
            Debug.LogError("Player must register before assigning score");
            return;
        }
        var player = _players[i];
        player.Score = _players[i].Score.Increment(type);
        _players[i] = player;
        _players.Dirty(i);
        _changeCounter++;
    }

    internal IEnumerable<Player> GetPlayers()
    {
        return _players.OrderByDescending(player => player.Score.Kills);
    }

    [Server]
    public void RegisterPlayer(string id, string name)
    {
        _players.Add(new Player(id, name));
        _changeCounter++;
    }

    public int GetChangeCounter()
    {
        return _changeCounter;
    }
}