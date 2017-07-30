using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;

namespace WAR.Game.Tests {
	public class GamePhaseTest {
		[Test]
		public void PhaseTransition() {
			var go = new GameObject();
			// create a WARGame object to store the game phase
			var game = go.AddComponent<WARGame>() as WARGame;
			WARGame.Instance = game;
			// and out gameplay controller to step through the phases
			var gameplay = go.AddComponent<WARModeGameplay>() as WARModeGameplay;
			
			// the phase we are starting in
			GAME_PHASE startPhase = GAME_PHASE.movement;
			// the phase we expect to transition to
			GAME_PHASE expectedPhase =	GAME_PHASE.command;
			
			// set the phase to our start phase
			WARGame.SetPhase(startPhase);
			// move to the next phase
			gameplay.nextPhase();
			
			// make sure we got the desired phase transition
			Assert.AreEqual(expectedPhase, WARGame.Phase.Value.current);
			// destroy the gameplay controller to clean up subscriptions
			gameplay.OnDestroy();
		}
		[Test]
		public void PhaseEnd() {
			var go = new GameObject();
			// create a WARGame object to store the game phase
			var game = go.AddComponent<WARGame>() as WARGame;
			WARGame.Instance = game;
			// and out gameplay controller to step through the phases
			var gameplay = new GameObject().AddComponent<WARModeGameplay>() as WARModeGameplay;
			WARModeGameplay.Instance = gameplay;
			gameplay.Start();
			
			// create players to add to our game
			WARGame.Players.Add(new WARPlayer(1));
			WARGame.Players.Add(new WARPlayer(2));
			
			// the phase we are starting in
			GAME_PHASE startPhase = GAME_PHASE.morale;
			// the phase we expect to transition to
			GAME_PHASE expectedPhase =	GAME_PHASE.movement;
			
			// set the phase to our start phase
			WARGame.SetPhase(startPhase);
			// move to the next phase
			gameplay.nextPhase();
			
			// make sure we got the desired phase transition
			Assert.AreEqual(expectedPhase, WARGame.Phase.Value.current);
			// destroy the gameplay controller to clean up subscriptions
			gameplay.OnDestroy();
		}
		[Test]
		public void TurnTransition() {
			var go = new GameObject();
			// create a WARGame object to store the game phase
			var game = go.AddComponent<WARGame>() as WARGame;
			WARGame.Instance = game;
			// and out gameplay controller to step through the phases
			var gameplay = new GameObject().AddComponent<WARModeGameplay>() as WARModeGameplay;
			WARModeGameplay.Instance = gameplay;
			gameplay.Start();
		
			// create players to add to our game
			WARGame.Players.Add(new WARPlayer(1));
			WARGame.Players.Add(new WARPlayer(2));
			
			int currentPlayer = 1;
			int targetPlayer = 2;
			
			// set the current player
			WARGame.CurrentPlayer = currentPlayer;
			// set the phase to our start phase
			WARGame.SetPhase(GAME_PHASE.morale);
			// move to the next phase
			gameplay.nextPhase();
			
			// make sure we got the desired phase transition
			Assert.AreEqual(targetPlayer, WARGame.CurrentPlayer);
			// destroy the gameplay controller to clean up subscriptions
			gameplay.OnDestroy();
		}
		[Test]
		public void RoundEnd() {
			var go = new GameObject();
			// create a WARGame object to store the game phase
			var game = go.AddComponent<WARGame>() as WARGame;
			WARGame.Instance = game;
			// and out gameplay controller to step through the phases
			var gameplay = new GameObject().AddComponent<WARModeGameplay>() as WARModeGameplay;
			WARModeGameplay.Instance = gameplay;
			gameplay.Start();
			
			// create players to add to our game
			WARGame.Players.Add(new WARPlayer(1));
			WARGame.Players.Add(new WARPlayer(2));
			
			int currentPlayer = 2;
			int targetPlayer = 1;
			
			// set the current player
			WARGame.CurrentPlayer = currentPlayer;
			
			// set the phase to our start phase
			WARGame.SetPhase(GAME_PHASE.morale);
			// move to the next phase
			gameplay.nextPhase();
			
			// make sure we got the desired phase transition
			Assert.AreEqual(targetPlayer, WARGame.CurrentPlayer);
			// destroy the gameplay controller to clean up subscriptions
			gameplay.OnDestroy();
		}
	}
}
