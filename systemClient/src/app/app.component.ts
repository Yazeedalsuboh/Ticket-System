import { Component, computed, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';

import { HeaderComponent } from './shared/components/header/header.component';
import { ApiService } from './shared/services/api.service';
import { ChatBoxComponent } from './shared/components/chat-box/chat-box.component';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatButtonModule,
    HeaderComponent,
    ChatBoxComponent,
    NgIf,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  readonly _apiService = inject(ApiService);
  profile = computed(() => this._apiService.profile());

  ngOnInit(): void {
    this._apiService.refreshProfile();
  }
}
