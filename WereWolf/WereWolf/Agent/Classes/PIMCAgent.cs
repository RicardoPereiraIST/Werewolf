using System;
using System.Collections.Generic;
using System.Linq;
using WereWolf.Nodes;
using WereWolf.General;

namespace WereWolf
{
    public class PIMCAgent : Agent
    {
        private InformationSet infoSet;
        private Player player;
        private bool liar;

        public PIMCAgent(Player player, bool isLiar)
        {
            infoSet = new InformationSet(player.getPlayerName(), isLiar);
            this.player = player;
            this.liar = isLiar;
        }

        public void setPlayersList(List<String> p)
        {
            infoSet.setPlayersList(p);
        }

        public void killPlayer(string playerName)
        {
            infoSet.addKillPlay(playerName);
        }

        public void addFriend(string friend)
        {
            infoSet.addFriend(friend);
        }

        public void addTalk(string talker, string playerName, string role)
        {
            infoSet.addTalk(talker, playerName, role);
        }

        public void addSave(string playerName)
        {
            infoSet.addSave(playerName);
        }

        public void addRole(String playerName, String playerRole)
        {
            infoSet.addRole(playerName, playerRole);
        }

        public string talkRound()
        {
            //Value of the talks, every one has a 0 value in the beginning.
            Dictionary<String, int> possibleTalks = infoSet.getPossibleTalks();

            for (int i = 0; i < Constants.N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample(player.getCharName()));

                RolloutGame game;
                List<String> possibleTalksCopy = new List<String>(possibleTalks.Keys);
                foreach (string possibleTalk in possibleTalksCopy)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.TALK, player.getCharName());
                    possibleTalks[possibleTalk] += game.sampleGame(possibleTalk);
                }
            }

            return possibleTalks.FirstOrDefault(x => x.Value == possibleTalks.Values.Max()).Key;
        }

        public string accuseRound()
        {
            Dictionary<String, int> possibleAccuses = infoSet.getPossibleAccuses();

            for (int i = 0; i < Constants.N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample(player.getCharName()));

                RolloutGame game;

                foreach (string possibleAccuse in infoSet.getPossibleAccuses().Keys)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.ACCUSE, player.getCharName());
                    possibleAccuses[possibleAccuse] += game.sampleGame(possibleAccuse);
                }
            }

            return possibleAccuses.FirstOrDefault(x => x.Value == possibleAccuses.Values.Max()).Key;
        }

        public string killRound()
        {
            Dictionary<String, int> possibleKills = infoSet.getPossibleKills();

            for (int i = 0; i < Constants.N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample(player.getCharName()));

                RolloutGame game;

                foreach (string possibleKill in infoSet.getPossibleKills().Keys)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.KILL, player.getCharName());
                    possibleKills[possibleKill] += game.sampleGame(possibleKill);

                }
            }

            return possibleKills.FirstOrDefault(x => x.Value == possibleKills.Values.Max()).Key;
        }

        public string healRound()
        {
            Dictionary<String, int> possibleHeals = infoSet.getPossibleHeals();

            for (int i = 0; i < Constants.N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample(player.getCharName()));

                RolloutGame game;

                foreach (string possibleHeal in infoSet.getPossibleHeals().Keys)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.HEAL, player.getCharName());
                    possibleHeals[possibleHeal] += game.sampleGame(possibleHeal);
                }
            }

            return possibleHeals.FirstOrDefault(x => x.Value == possibleHeals.Values.Max()).Key;
        }

        public string questionRound()
        {
            Dictionary<String, int> possibleQuestions = infoSet.getPossibleQuestions();

            for (int i = 0; i < Constants.N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample(player.getCharName()));

                RolloutGame game;

                foreach (string possibleQuestion in infoSet.getPossibleQuestions().Keys)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, GameStates.QUESTION, player.getCharName());
                    possibleQuestions[possibleQuestion] += game.sampleGame(possibleQuestion);
                }
            }

            return possibleQuestions.FirstOrDefault(x => x.Value == possibleQuestions.Values.Max()).Key;
        }

        public void accusePlayedRound(string playerName, string accusedPlayerName)
        {
            infoSet.addAccusePlay(playerName, accusedPlayerName);
        }

        public void seerQuestion(string playerName, string roleName)
        {
            infoSet.addSeerAnswer(playerName, roleName);
        }

        public void reinitializeBeliefs()
        {
            infoSet.reinitializeBeliefs();
        }
    }
}
