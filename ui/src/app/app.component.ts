import { Component } from '@angular/core';
import { PhoneBookService } from 'src/services/api.service';
import { IPhoneBook } from 'src/types/phonebook.interface';
import { IEntry, EntryModel } from 'src/types/entry.interface';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Phone Book App';
  phoneBook: IPhoneBook = null;
  name: string = '';
  phoneNumber: string = '';
  entryName: string = '';
  error: string = '';
  addingEntry: boolean = false;
  hasActive: boolean= false;
  entries: Array<EntryModel> = [];

  _filterString: string;
  get filterString(): string {
    return this._filterString;
  }
  set filterString(value: string) {
    this._filterString = value;
    this.entries = this.filterString ? this.filter(this.filterString) : this.phoneBook.entries;
  }

  constructor(private service: PhoneBookService) {
  }
  ngOnInit() {
    this.getAll();
  }

  getAll() {
    this.service.getAll().subscribe({
      next: (results: Array<IPhoneBook>) => {
        var book = results[0];
        if (book) {
          this.phoneBook = book;
          this.name = this.phoneBook.name;
          this.entries = this.phoneBook.entries;
        }
      },
      error: err => this.error = err
    });
  }

  reset() {

    this.error = '';
    this.phoneNumber = '';
    this.entryName = '';
    this.addingEntry = false;
    this.hasActive = false;
  }
  savePhoneBook() {
    this.reset();

    if (!this.phoneBook) {
      this.phoneBook = {
        id: "",
        name: this.name,
        entries: []
      }
    }
    else {
      this.phoneBook.name = this.name;

    }
    if (this.phoneBook.id.length === 0) {
      this.service.create(this.phoneBook).subscribe({
        next: () => this.getAll(),
        error: err => this.error = err
      });
    }
    else {
      this.service.update(this.phoneBook).subscribe({
        next: () => this.getAll(),
        error: err => this.error = err
      });
    }

  }
  createOrSave() {
    if (!this.entryName) {
      this.addingEntry = !this.addingEntry;
      return;
    }
    var entry: EntryModel = {
      id: "00000000-0000-0000-0000-000000000000",
      name: this.entryName,
      phoneNumber: this.phoneNumber,
      selected: false,
      deleteClass: ''
    };
    var index = this.phoneBook.entries.findIndex(e => e.id === entry.id);
    if (index != -1) {
      this.phoneBook.entries.splice(index, 1, entry);
    }
    else {
      this.phoneBook.entries.push(entry);
    }

    this.savePhoneBook();
  }
  filter(filterby: string): EntryModel[] {
    if (!filterby || filterby.length === 0) {
      return this.phoneBook.entries;
    }
    filterby = filterby.toLocaleLowerCase();
    return this.phoneBook.entries.filter((entry: EntryModel) => entry.name.toLocaleLowerCase().indexOf(filterby) !== -1);
  }
  setActive(index: number) {
    var entry = this.entries[index];
    this.entries.forEach(e => e.selected = false);
    entry.selected = !entry.selected;
    this.phoneNumber = entry.phoneNumber;
    this.entryName = entry.name;
    this.addingEntry = true;
    this.hasActive = entry.selected;

  }
  deleteEntry() {
    var entry = this.entries.find((e: EntryModel) => e.selected === true);
    if (!entry) {

      this.reset();
      return;
    }
    var index = this.phoneBook.entries.findIndex(e => e.id == entry.id);
    if (index === -1) {

      this.reset();
      return;
    }

    this.service.deleteEntry(this.phoneBook.id, entry.id).subscribe({
      next: () => {
        this.phoneBook.entries.splice(index, 1);
        this.entries = this.filter(this.filterString);
      },
      error: err => this.error = err
    });


    this.reset();
  }

  applyBlink() {
    var entry = this.entries.find((e: EntryModel) => e.selected === true);
    if (entry) {

      entry.deleteClass = " blink";
    }
  }
  removeBlink() {

    var entry = this.entries.find((e: EntryModel) => e.selected === true);
    if (entry) {
      entry.deleteClass = "";

    }
  }
}
