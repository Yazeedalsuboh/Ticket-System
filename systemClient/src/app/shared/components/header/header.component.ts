import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, RouterLinkActive } from '@angular/router';
import {
  TranslateModule,
  TranslatePipe,
  TranslateService,
  _,
} from '@ngx-translate/core';
import { ApiService } from '../../services/api.service';
import { CommonModule } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { UserRoles } from '../../types/users-types';
import { AiCohereService } from '../../services/ai-cohere.service';

@Component({
  selector: 'app-header',
  imports: [
    TranslateModule,
    MatIconModule,
    MatButtonModule,
    RouterLink,
    TranslatePipe,
    CommonModule,
    RouterLinkActive,
    MatMenuModule,
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  private readonly translate = inject(TranslateService);
  private readonly _apiService = inject(ApiService);

  UserRoles = UserRoles;
  isDark = signal<boolean>(false);

  profile = computed(() => this._apiService.profile());

  toggleTheme() {
    this.isDark.update((val) => !val);
    document.documentElement.classList.remove('dark-theme', 'light-theme');
    document.documentElement.classList.add(
      this.isDark() ? 'dark-theme' : 'light-theme'
    );
    localStorage.setItem('theme', this.isDark() ? 'dark' : 'light');
  }

  ngOnInit(): void {
    const theme = localStorage.getItem('theme') || 'light';
    const isDark = theme === 'dark';
    this.isDark.set(isDark);

    document.documentElement.classList.remove('dark-theme', 'light-theme');
    document.documentElement.classList.add(
      isDark ? 'dark-theme' : 'light-theme'
    );
  }

  logout() {
    this._apiService.logout();
  }

  getLink(): string {
    return this.profile().role === UserRoles.Manager
      ? '/dashboard'
      : '/tickets/list';
  }

  translateText() {
    if (this._apiService.currentLang() == 'en') {
      this.translate.use('ar');
      this._apiService.currentLang.set('ar');
    } else {
      this.translate.use('en');
      this._apiService.currentLang.set('en');
    }
  }
}
