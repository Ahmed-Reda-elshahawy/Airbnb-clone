import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatMessage, SendMessageRequest } from '../models/ChatMessage';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private baseUrl = `${environment.apiUrl}/chat`;

  constructor(private http: HttpClient) { }

  // Get conversation by ID
  getConversation(conversationId: string): Observable<ChatMessage[]> {
    return this.http.get<ChatMessage[]>(`${this.baseUrl}/conversation/${conversationId}`);
  }

  // Send message
  sendMessage(request: SendMessageRequest): Observable<ChatMessage> {
    return this.http.post<ChatMessage>(`${this.baseUrl}/send`, request);
  }

  // Create new conversation
  createConversation(): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/conversation`, {});
  }

  getRecentConversations(): Observable<ChatMessage[]> {
    return this.http.get<ChatMessage[]>(`${this.baseUrl}/conversations/recent`);
  }
  
}