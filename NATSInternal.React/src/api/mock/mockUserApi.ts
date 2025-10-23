import { useMockDatabase } from "./mockDatabase";
import type { UserApi } from "../userApi";

const mockDatabase = useMockDatabase();

const userApi: UserApi = {
  getListAsync: async (requestDto) => {
    const result
    const query = () => {
      switch (requestDto.sortByFieldName) {
        case "CreatedDateTime":
          
      }
    }
  } 
};