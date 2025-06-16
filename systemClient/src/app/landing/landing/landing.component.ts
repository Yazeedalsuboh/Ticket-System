import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing',
  imports: [MatCardModule, MatIconModule, MatButtonModule, RouterLink],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
})
export default class LandingComponent {}
