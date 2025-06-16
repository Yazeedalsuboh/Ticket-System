import { CommonModule, NgClass, NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  NgModel,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInput, MatInputModule } from '@angular/material/input';
import { AiCohereService } from '../../services/ai-cohere.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-chat-box',
  imports: [
    NgClass,
    NgIf,
    MatIconModule,
    CommonModule,
    ReactiveFormsModule,
    MatInput,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
  ],
  templateUrl: './chat-box.component.html',
  styleUrl: './chat-box.component.css',
})
export class ChatBoxComponent {
  chatForm: FormGroup;
  visible = false;
  messages: { text: string; from: 'user' | 'bot' }[] = [];
  readonly _aiService = inject(AiCohereService);

  @ViewChild('scrollContainer') scrollContainer!: ElementRef;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.chatForm = this.fb.group({
      message: [''],
    });
  }

  toggle() {
    this.visible = !this.visible;
  }

  sendMessage() {
    if (this.chatForm.invalid) return;

    const userText = this.chatForm.value.message;
    if (userText.trim().length) {
      this.messages.push({ text: userText, from: 'user' });
      this.chatForm.reset();
      this.scrollToBottom();

      this._aiService
        .chat(userText)
        .then((reply) => {
          this.messages.push({ text: reply, from: 'bot' });
          this.scrollToBottom();
        })
        .catch((error) => {
          this.messages.push({
            text: '⚠️ Failed to get response.',
            from: 'bot',
          });
          this.scrollToBottom();
        });
    }
  }

  private scrollToBottom() {
    setTimeout(() => {
      if (this.scrollContainer) {
        this.scrollContainer.nativeElement.scrollTop =
          this.scrollContainer.nativeElement.scrollHeight;
      }
    });
  }
}
