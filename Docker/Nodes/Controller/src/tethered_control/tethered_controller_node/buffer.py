# import torch
# import torch.nn as nn
# import torch.nn.functional as f
import numpy as np

class ReplayBuffer():
    def __init__(self, state_dim, action_dim, estimate_dim, device=None, buffer_length=int(1e5)):
        self.buffer_length = buffer_length
        self.ptr = 0
        self.size = 0


        self.prev_estimates = np.zeros((self.buffer_length, estimate_dim))
        self.states = np.zeros((self.buffer_length, state_dim))
        self.next_estimates = np.zeros((self.buffer_length, estimate_dim))
        self.actions = np.zeros((self.buffer_length, action_dim))
        self.device = device
    
    #Adding single datapoint, assumes sequential order from previous.
    #prev_estimate = estimate at time step t
    #state = states at time step t-H: t
    #action = actions at time steps t-H: t
    #estimate = estimate for time step t+ 1
    def add(self, prev_estimate, states, actions, estimate):
        self.prev_estimates[self.ptr] = prev_estimate
        self.states[self.ptr] = states
        self.next_estimates[self.ptr] = estimate
        self.actions[self.ptr] = actions

        prev_ptr = self.ptr
        self.ptr += 1 % self.buffer_length
        self.size = min(self.size + 1, self.buffer_length)


    #Samples entire memory of Single state, action pairs.
    def sample(self, index):
        estimates = self.prev_estimates[index]
        states = self.states[index]
        actions = self.actions[index]
        next_estimates = self.next_estimates[index]

        # return (
        #     torch.FloatTensor(estimates).to(self.device), 
        #     torch.FloatTensor(states).to(self.device), 
        #     torch.FloatTensor(actions).to(self.device),
        #     torch.FloatTensor(next_estimates).to(self.device)
        # )
    
    def save(self, folder):
        print(f"saving {self.size} samples!")
        np.save(f'{folder}/states.npy', self.states[:self.size])
        np.save(f'{folder}/estimates.npy', self.prev_estimates[:self.size])
        np.save(f'{folder}/next_estimates.npy', self.next_estimates[:self.size])
        np.save(f'{folder}/actions.npy', self.actions[:self.size])

    def load(self):
        pass



        
