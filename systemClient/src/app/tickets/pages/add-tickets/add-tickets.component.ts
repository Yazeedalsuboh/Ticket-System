import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TicketsService } from '../../../shared/services/tickets.service';
import { MatCardModule } from '@angular/material/card';
import { TranslatePipe } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { MatChipsModule } from '@angular/material/chips';
import { Product } from '../../../shared/types/products-types';
import { Ticket } from '../../../shared/types/tickets-types';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AiCohereService } from '../../../shared/services/ai-cohere.service';

@Component({
  selector: 'app-add-tickets',
  imports: [
    MatCardModule,
    TranslatePipe,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    CommonModule,
    MatOptionModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    AsyncPipe,
    MatTooltipModule,
  ],
  templateUrl: './add-tickets.component.html',
  styleUrl: './add-tickets.component.css',
})
export default class AddTicketsComponent implements OnInit {
  private readonly _formBuilder = inject(FormBuilder);
  readonly _ticketsService = inject(TicketsService);
  private readonly _router = inject(Router);
  readonly _aiService = inject(AiCohereService);

  products$!: Observable<Product[]>;

  ticket = signal<Ticket | null>(null);

  async ngOnInit() {
    this.products$ = this._ticketsService.getProducts();

    if (this._ticketsService.toUpdateId().length) {
      this._ticketsService
        .byId(this._ticketsService.toUpdateId())
        .subscribe(async (response: Ticket) => {
          this.ticket.set(response);

          const fields = ['title', 'description', 'productName'];

          fields.forEach((field) => {
            this.ticketForm.get(field)?.setValue((response as any)[field]);
          });

          const filePromises = response.attachments.map(
            async (ele: { fileUrl: string; fileName: string }) => {
              return await this.urlToFile(ele.fileUrl, ele.fileName);
            }
          );

          const files = await Promise.all(filePromises);
          this.ticketForm.get('attachments')?.setValue(files);
        });
    }
  }

  async urlToFile(imageUrl: string, fileName: string): Promise<File> {
    const response = await fetch(imageUrl);
    const blob = await response.blob();
    const contentType = blob.type || 'image/jpeg';

    return new File([blob], fileName, { type: contentType });
  }

  ticketForm = this._formBuilder.group(
    {
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      productName: ['', Validators.required],
      attachments: [[] as File[]],
      priority: [''],
    },
    { NonNullableFormBuilder: true }
  );

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const existingFiles = this.ticketForm.get('attachments')?.value || [];
      const newFile = input.files[0];
      this.ticketForm.get('attachments')?.setValue([...existingFiles, newFile]);
    }
  }

  removeFile(fileName: string): void {
    const filesControl = this.ticketForm.get('attachments');
    const existingFiles: File[] = filesControl?.value || [];
    const updatedFiles = existingFiles.filter((file) => file.name !== fileName);
    filesControl?.setValue(updatedFiles);
  }

  submit() {
    if (this._ticketsService.toUpdateId().length) {
      this.ticketForm.get('priority')?.setValue(this.ticket()?.priority);
      this._ticketsService.update(this.ticketForm.value).subscribe({
        next: (res) => {
          if (res.success) {
            this.ticketForm.reset();
            this._ticketsService.toUpdateId.set('');
            this._router.navigate(['/tickets/list']);
          }
        },
      });
    } else {
      this._ticketsService.add(this.ticketForm.value).subscribe({
        next: (res) => {
          if (res.success) {
            this.ticketForm.reset();
            this._router.navigate(['/tickets/list']);
          }
        },
      });
    }
  }

  translateText(key: string) {
    this._aiService
      .translateText(this.ticketForm.get(key)?.value)
      .then((reply) => {
        this.ticketForm.get(key)?.setValue(reply);
      })
      .catch((error) => {
        console.error(error);
      });
  }
}
