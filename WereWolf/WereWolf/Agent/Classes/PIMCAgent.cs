using System;
using System.Collections.Generic;
using System.Linq;
using WereWolf.Nodes;

namespace WereWolf
{
    public class PIMCAgent : Agent
    {
        private InformationSet infoSet;
        private const int N = 1;
        private Player player;
        private bool liar;

        public PIMCAgent(Player player)
        {
            infoSet = new InformationSet(player.getPlayerName());
            this.player = player;
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

        public void updateBeliefs()
        {
            infoSet.updateBeliefs();
        }

        public string talkRound()
        {
            //Value of the talks, every one has a 0 value in the beginning.
            Dictionary<String, int> possibleTalks = infoSet.getPossibleTalks();

            for (int i = 0; i < N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample());

                RolloutGame game;

                foreach (string possibleTalk in infoSet.getPossibleTalks().Keys)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.TALK);
                    game.sampleGame(string.Format("talk {0}", possibleTalk));
                    possibleTalks[possibleTalk] += game.evalGame(player.getCharName());
                }
            }

            return string.Format("talk {0}", possibleTalks.FirstOrDefault(x => x.Value == possibleTalks.Values.Max()).Key);
        }

        public string accuseRound()
        {
            Dictionary<String, int> possibleAccuses = infoSet.getPossibleAccuses();

            for (int i = 0; i < N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample());

                RolloutGame game;

                foreach (string possibleAccuse in infoSet.getPossibleAccuses().Keys)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.ACCUSE);
                    game.sampleGame(possibleAccuse);
                    possibleAccuses[possibleAccuse] += game.evalGame(player.getCharName());
                }
            }

            return possibleAccuses.FirstOrDefault(x => x.Value == possibleAccuses.Values.Max()).Key;
        }

        public string killRound()
        {
            Dictionary<String, int> possibleKills = infoSet.getPossibleKills();

            for (int i = 0; i < N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample());

                RolloutGame game;

                foreach (string possibleKill in infoSet.getPossibleKills().Keys)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.KILL);
                    possibleKills[possibleKill] += game.sampleGame(possibleKill);

                }
            }

            return possibleKills.FirstOrDefault(x => x.Value == possibleKills.Values.Max()).Key;
        }

        public string healRound()
        {
            Dictionary<String, int> possibleHeals = infoSet.getPossibleHeals();

            for (int i = 0; i < N; i++)
            {
                List<PlayerNode> accuseSample = new List<PlayerNode>();
                accuseSample.Add(new MaxNode(player.getPlayerName(), player.getCharName(), infoSet));
                accuseSample.AddRange(infoSet.Sample());

                RolloutGame game;

                foreach (string possibleHeal in infoSet.getPossibleHeals().Keys)
                {
                    List<PlayerNode> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.HEAL);
                    game.sampleGame(string.Format("heal {0}", possibleHeal));
                    possibleHeals[possibleHeal] += game.evalGame(player.getCharName());
                }
            }

            return string.Format("heal {0}", possibleHeals.FirstOrDefault(x => x.Value == possibleHeals.Values.Max()).Key);
        }

        public string questionRound()
        {
            Dictionary<String, int> possibleQuestions = infoSet.getPossibleQuestions();

            for (int i = 0; i < N; i++)
            {
                string talkSample = infoSet.questionSample();
                RolloutGame game;

                foreach (string possibleQuestion in infoSet.getPossibleQuestions().Keys)
                {

                }
            }

            return string.Format("question {0}", possibleQuestions.FirstOrDefault(x => x.Value == possibleQuestions.Values.Max()).Key);
        }

        public void accusePlayedRound(string playerName, string accusedPlayerName)
        {
            infoSet.addAccusePlay(playerName, accusedPlayerName);
        }

        public void seerQuestion(string playerName, string roleName)
        {
            infoSet.addSeerAnswer(playerName, roleName);
        }
    }
}
