import { IEntry, EntryModel } from './entry.interface';

export interface IPhoneBook
{
    id:string,
    name:string,
    entries: Array<EntryModel>
}