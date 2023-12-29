
import { Home } from "./components/Home";
import { InfoView } from "./components/InfoView";
import { URLInfo } from "./components/URLInfo.jsx";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/urls',
    element: <URLInfo />
  },
  {
    path: '/info',
    element: <InfoView />
  }
];

export default AppRoutes;
