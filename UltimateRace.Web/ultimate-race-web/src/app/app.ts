import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Race } from './race/race';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Race],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('ultimate-race-web');
}
