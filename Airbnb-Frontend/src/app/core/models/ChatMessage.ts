export interface ChatMessage {
    text: string;
    sender: 'user' | 'bot';
    timestamp: Date;
}

export interface SendMessageRequest {
    message: string;
    conversationId: string;
}