import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../../core/services/chat.service';
import { ChatMessage } from '../../core/models/ChatMessage';

@Component({
  selector: 'app-chat-bot',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat-bot.component.html',
  styleUrls: ['./chat-bot.component.css']
})
export class ChatBotComponent implements OnInit {
  isOpen = false;
  newMessage = '';
  messages: ChatMessage[] = [];
  currentConversationId: string | null = null;

  constructor(private chatService: ChatService) {}

  ngOnInit() {
    // Create a new conversation when the component initializes
    this.createNewConversation();
  }

  private createNewConversation() {
    this.chatService.createConversation().subscribe({
      next: (conversationId) => {
        this.currentConversationId = conversationId;
        // Load initial conversation
        this.loadConversation();
      },
      error: (error) => {
        console.error('Error creating conversation:', error);
      }
    });
  }

  private loadConversation() {
    if (this.currentConversationId) {
      this.chatService.getConversation(this.currentConversationId).subscribe({
        next: (messages) => {
          this.messages = messages;
        },
        error: (error) => {
          console.error('Error loading conversation:', error);
        }
      });
    }
  }

  toggleChat() {
    this.isOpen = !this.isOpen;
  }

  sendMessage() {
    if (!this.newMessage.trim()) {
      return;
    }

    if (!this.currentConversationId) {
      // If no conversation exists, create one first
      this.chatService.createConversation().subscribe({
        next: (conversationId) => {
          this.currentConversationId = conversationId;
          this.sendMessageToApi();
        },
        error: (error) => {
          console.error('Error creating conversation:', error);
        }
      });
    } else {
      this.sendMessageToApi();
    }
  }

  private sendMessageToApi() {
    const request = {
      conversationId: this.currentConversationId!,
      message: this.newMessage.trim()
    };

    // First add the user message to the UI
    this.messages.push({
      text: this.newMessage,
      sender: 'user',
      timestamp: new Date()
    });

    // Clear the input field
    this.newMessage = '';

    // Send the message to the API
    this.chatService.sendMessage(request).subscribe({
      next: (response) => {
        this.messages.push(response);
      },
      error: (error) => {
        console.error('Error sending message:', error);
        // Add an error message to the chat
        this.messages.push({
          text: 'Sorry, there was an error sending your message. Please try again.',
          sender: 'bot',
          timestamp: new Date()
        });
      }
    });
  }

  endConversation() {
    // Create a new conversation
    this.messages = [];
    this.createNewConversation();
    this.isOpen = false;
  }
  showRecentConversations(){
    this.chatService.getRecentConversations().subscribe({
      next: (conversations: ChatMessage[]) => {
        // Update messages with the recent conversations
        this.messages = conversations;
      },
      error: (error) => {
        console.error('Error loading recent conversations:', error);
      }
    });

  }
}
