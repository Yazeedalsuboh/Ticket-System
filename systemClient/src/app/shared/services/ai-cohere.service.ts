import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { CohereClientV2 } from 'cohere-ai';
import { TicketsService } from './tickets.service';
import { GeneralService } from './general.service';

@Injectable({
  providedIn: 'root',
})
export class AiCohereService {
  private readonly _httpClient = inject(HttpClient);
  private readonly _apiKey: string = environment.AIapiKey;
  readonly _ticketsService = inject(TicketsService);
  private readonly _generalService = inject(GeneralService);

  async enhanceTicket(ticket: any, key: string, id: string) {
    this._ticketsService.toUpdateId.set(id);
    try {
      const cohere = new CohereClientV2({ token: this._apiKey });
      const response = await cohere.chat({
        model: 'command-a-03-2025',
        messages: [
          {
            role: 'user',
            content: `Improve the clarity and professionalism of this ${key}, return only the improved ${key}:\n${ticket[key]}`,
          },
        ],
      });

      if (response.finishReason === 'COMPLETE' && response.message.content) {
        ticket[key] = response.message.content[0].text;
        this._ticketsService.update(ticket).subscribe({
          next: (response) => {
            if (response.success) {
              this._generalService.openSnackBar(`${key} enhanced successfully`);
            }
          },
        });
        return response.message.content[0].text;
      }
      throw new Error('Invalid response from Cohere API');
    } catch (error) {
      console.error('Title enhancement failed:', error);
      throw error;
    }
  }

  async chat(message: string): Promise<string> {
    try {
      const cohere = new CohereClientV2({ token: this._apiKey });
      const response = await cohere.chat({
        model: 'command-a-03-2025',
        messages: [
          {
            role: 'user',
            content: message,
          },
        ],
      });

      if (response.finishReason === 'COMPLETE' && response.message.content) {
        return response.message.content[0].text;
      }
      throw new Error('Invalid response from Cohere API');
    } catch (error) {
      console.error('Title enhancement failed:', error);
      throw error;
    }
  }

  async translateText(text: string): Promise<string> {
    try {
      const cohere = new CohereClientV2({ token: this._apiKey });
      const response = await cohere.chat({
        model: 'command-a-03-2025',
        messages: [
          {
            role: 'user',
            content: `Translate the text to english if its in arabic and to arabic if its in english, return only the text, Text:${text}`,
          },
        ],
      });

      if (response.finishReason === 'COMPLETE' && response.message.content) {
        return response.message.content[0].text;
      }
      throw new Error('Invalid response from Cohere API');
    } catch (error) {
      console.error('Title enhancement failed:', error);
      throw error;
    }
  }

  async determinePriority(ticket: any, id: string) {
    this._ticketsService.toUpdateId.set(id);
    try {
      const cohere = new CohereClientV2({ token: this._apiKey });
      const response = await cohere.chat({
        model: 'command-a-03-2025',
        messages: [
          {
            role: 'user',
            content: `Determine the proiorty of this ticket (High, Low) depending on the title and the description, return only High or Low, Title: ${ticket.title}, Description: ${ticket.description} `,
          },
        ],
      });

      if (response.finishReason === 'COMPLETE' && response.message.content) {
        ticket.priority = response.message.content[0].text;
        this._ticketsService.update(ticket).subscribe({
          next: (response) => {
            if (response.success) {
              this._generalService.openSnackBar(`Priority Updates`);
            }
          },
        });
        return response.message.content[0].text;
      }
      throw new Error('Invalid response from Cohere API');
    } catch (error) {
      console.error('Title enhancement failed:', error);
      throw error;
    }
  }
}
